
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

It's a bit of shitshow I won't lie. I'm almost embarressed. It does work, I unit tested it and every thing (also theres no actual user string input so now sql injection), but idk, I did look around couldn't find an obvious way to do it better, if it wasn't performent I might just rewrite it in C.

Something I am proud of in the GeoLocation feature. A user puts in there PostCode and that is converted into coordinates and put in the Database. Well Not really...
Reverse geoding requires like a Bing maps api or something, I tried to sign up but I think you have to be an actual company, at the very least they are ratelimited, and as this is a toy app I though it was an unessesary hurdle.
Instead, i made a class that makes polygon with an upper and lower bound of lat and long with encomposs the greater london area. "Updating" your location basically just generate a random set of coordiates. 

When you search, you specify the range from yourself, which it then compares to the coordinates of the search results, and filters those out of range. 

## Rota/Messages/Contracts

So I've grouped these projects mainly on dependancies, and respective concerns. Accounts uses EFCore, Profile uses Dapper.
these service all use both Redis and Neo4j in combination.
I've used redis as a db rather than a cache.

From what I understand about graph dbs, they're awesome at two things, ManyToMany relationships, and traversal across related data (aka, "find me all of the people who are friends with the friends of alice", would seriously tax sql, but would be trival for a graph). But I also understand they're not optimised for actually storing lots of data on the nodes themselves. 

So basically where needed I've stored some identifer of data on the graph, and the values themselves in redis.
For example:
*node*     *relationship*
 (JeanClaudeVannDamme{Id:JcvdId})<- Message_Thread -> (Bob{Id:BobId}) <- Message_Thread -> (Alice{Id:AliceId})

query: get all the ids, from the nodes connected via a Message_Thread, to the node whos Id = BobId.
returns: [AliceId, JcvdId]

With the ids, go to redis and get message threads stored at messagePrefix+id1+id2

It's kinda convoluted ngl, but it doesn mean that there is only one copy of the Message thread that both parties have access to.
There is undoubtly better ways to do it I admit.


# Messages

Opted not to go for signlR, this isn't a chat app, it's aimed solving at a very narrow business problem. Realistically, the only utility of messages are just to get each others whatapps, after vetting the other person is sane enough to offer such. Thus real-time communication seemed a bit ott. Honestly one gripe I have about a lot of everyday software like apps, is that they have way to many features, I downloaded a pomdoro timer the other day, and it came with a fucking manual. Bro all I wanted was a clock.

MongoDb would be a more traditional choice, but redis can be used as a db and the only action is to append a message onto a list, I think it does the job. Also I couldn't be asked to learn 5th database. 

Moreover, message threads are emphemeral (doubt i spelled that right), they really would just be used to get a whatsapp, number, so there is no utility storing them longer than a month.

Flow is this:

partyA sends message request =>
  other party responds (accepted/rejected/block) =>
    if the formost, now they can send messages


      

# Contracts

pretty basic crud, all on the Neo4j.
request contract =>
  confirm/deny => 
    get contracts
    delete contract etc etc



# Rota

This isn't finished. I know what to do but its i just cba.

a hosted services on a one day timer calls the main function to delete the outdated shift, add a new rota day X days from now, and form a new shift for everyone.

A bouncer will specifiy the upcoming days they want to work (defaulting to none), and the max start and end times.
A club will do the same-ish, additionally specifiying personal needed, etc.

the users can change these up until the day the rota is made.
the values of each are used to make the rotas.

obvs bouncers can pull out, updatin there status on an assigned shift, or clubs can kick them.
but there is no redoing the rota.

Shifts are assigned, but can be dropped by either party. shifts cant be updated to add someone, they can just use whatsapp. Again this isn't an Event-Managment app. It does one thing (if that).




# General Stuff
I wanted to make everything super functional but doing with while everything with asyncronous was just to annoying.













