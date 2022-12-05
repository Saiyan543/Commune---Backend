
Matt, this is just a rough explanation.

# What it is
A only social-media like app, the aim of which is connect bouncers with clubs etc who need to source them, with the app acting as a mediator, with some other things. 
(I was a bouncer in uni and it really is a huge issue, the jist is that the industry standard for sourcing is essentially a wild west)

# The Idea

- You sign up as either a bouncer or a club
- You query profiles based on set critera, aka: all every bouncer who works saturdays within 10 km of location X. Currently the critera are pretty limited because but gonna expand on it, for example, instead of just "saturday", it might be saturday mornings/evenings/nights. 
- Profiles are pretty basic, with a bio, I considered a rating system but thought it would be pretty crass, and honestly when "interviewing" for a bouncer job the only questions i've ever been asked are "can you do Saturdays".
- You can send a request message to chat, and chat further if it's accepted. from there you can send a "contract" request between a bouncer and club (to be accepted/denied etc). The contract is actually more akin to a friend request, as there is no legal rammification, and can be deleted at any time by eiter party. 
- The contract's purpose is to do with the Rota ...

# Rota
So you sign up, find some bouncers/clubs you'd like to work with, send them a message, link via a contract. 
The contract exists as a link between nodes (security node or club node) in a graph database. Each user will specify the days and times they are available/need to work, and for clubs also the number of personel they need for each date. On a regular one day interval, each users info on a specific date in the future (aka 7 days from now), will be evaulated, and will be used, as well as the contracts to randomly assigned bouncers to clubs.  
The basic flow:
Get all clubs => 
  filter clubs that are open on day X, and shuffle them randomly => 
    for club at i, get all bouncers who have contracts with them, not already assigned =>
      filter bouncers on club[i].starttime >= bouncer.starttime && working, endtime etc =>
        Create a shift object that describes the club info, bouncers info etc (date, time, ids, is the bouncer confirmed?) =>
          store that somewhere give it an id regarding the date aka, clubId + DateTime.Now.ToString()
    Repeat for each club.
    
# why
so an important feature of this not yet described is that if a bouncer and a club have a contract (again, basically a friend request), then not only will the bouncer be considered for a shift at that club, but further, clubs and have contracts between them that allow a bouncer to be condidered for that shift as well:
 *node*        *relationship*
(Bouncer)-Has_Contract_With-(ClubA)-Has_Contract_With-(ClubB)

so if ClubA on a given night has all it's personel filled, and ClubB doesn't, a second round of search will extend through the (club)-(club) relationship to match Bouncer and ClubB


# This sounds a little sketchy, like, what club would be okay with this? all of them.
I'll go into some (in the appendix ofcourse) more detail about this in my actual write up, but basically, clubs source through multiple agencies, because of the massive increase in demand for security in the past few years, the bar is effectivly on the floor. If you apply on indeed and have a valid SIA licience, you will be offered a shift by the end of the day.

Obviously the problem with this is that as agencies are phiscally motivated to fill shifts you get some wrongungs, oh boy. 

The way in which my system tries to solve this is quite subtle. It doesn't directly solve anything, rather it encourages behaviour that leads to the problem being solved.

# to sum up the problem
Clubs continuely short of bouncers, so they accept anything agencies throw at them, due to the quality, clubs have a continual rotating door of bouncers, and cling on to dear life to the good ones.

# f

Essentially, the solution is for clubs who geographically grouped to share security on one large rota, and on shifts coordinate as such.

I was a part of a system like this when I worked in Bath, where one agency pretty much had a monopoly on the centre (and were actually decent guys), we were assigned a club, but had a communal radio, protocals to converge on a place if it was needed. And guys to who scout the town warning us about "tall guy, arsenel shirt, bald. he was doing coke in the alley, said he was coming to his missus".

- With this system a club doesn't have to stress about not having enough staff on occasion, because worst case scenario help will arrive in a few minutes. thus the bar doesn't have to be quite so low, and turnover might slow.

- The only real considerations for a bouncer doing work are: commute, the amount of shit they have to deal with, and the quality of the other bouncers. A bouncer who continually works at one of say 5 locations, all within ear shot of each other, is, catagorically, nicer than being at which ever place Joe's Sercurity Agency chucks you. I think the benifits to the bouncer of working within an ecosystem are self evident, again in my actual write up ill be more rigourous... Boobs. 

- From the clubs perspective again, a dynamic push and pull of staff to near by locations when and if they are needed. (It's important to know that a bouncers time is split such that 90% is spent standing around doing fuck all, 8% is telling drunk people they can't come in/must leave, and 2% in fight. Thus the idea of a some staff leaving to go accross the street to de-escalate a situation shouldn't be an issue for clubs.
A clubs rota of bouncers is more consistent, with more experience working with each other, and yeah.


This is something that could be accomplished with a third party (this app), but, as i've heard personally from club managers, if they find a good bouncer, they don't want to share. So I think that haveing an unbiased mediator is a solution. Further, the app actually punishes selfishness. Not cooperating with other clubs only leaves them worse off. 

Theres more I could say but i'm sure you're thoroughly convinced.

## TECH

So I origonally did microservices, but it felt a bit stupid because there was so much duplicated code. Which obviously makes sense for teams of devs but it just kinda pointless. Didn't want to do a classical monolyth though, so I researched and found something called virtual-slice architecture https://www.youtube.com/watch?v=lsddiYwWaOQ&t=471s
basically within a single project file, multiple sub projects, but keep them decoupled (at least temporally). So they'd all shared dependancies like an Error handler, or authorisation logic, but keep stuff like Database dependancies seperate. Still Onion style:

Project
 - Global dependancies, Functional classes
 - SubProject A - (Controllers => Services => Repository)
 - SubProject B - (Controllers => Services => Repository)
 - SubProject C - (Controllers => Services => Repository)



Mine has split into 3

# Accounts
Uses and ORM called EntityFrameworkCore, microsofts baby. its slow as shit but has great tools built-in identity management.
Auth is done with JWT and refresh tokens

Pretty much stores email, password, Tokens, etc. Private info between a user and the app.
it's also the, how do I say? like it's the source of truth?. Registering and deleting an account sends a messages to cascade through other databases respectively (via rabbitMq)

if(account deleted == success){
  publish message(location:Profile, action:delete, GlobalUserId:your_mum)
  publish message(location:Contracts, action:delete, GlobalUserId:your_mum)
}

# Profile
Uses the micro ORM dapper, it's a fast boi, requires raw sql and parameterised queries. I made an extension made extension methods for it (and Neo4j) that cut down the code duplication considerablly. 

The profile is what is presented to other users. A bio, days they are available, and really not much else. There was other things like "Months of experience" i've yet to add but wouldn't be hard.

obvs these searches are done via a query string (I made a class for that, does pageing etc), as dappers input is just a string of sql, I made a Class that dynamically builds a sql query and appends the search terms where applicable.

It's a bit of shitshow I won't lie. I'm almost embarressed. It does work, I unit tested it and every thing (also theres no actual user string input so now sql injection), but idk, I did look arpund couldn't find an obvious way to do it better, if it wasn't performant I might just rewrite it in C.






# General Stuff
I wanted to make everything super functional but doing with while everything with asyncronous was just to annoying.













