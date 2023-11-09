# Qwitter
A twitter like social media plattform with a purchasable premium feature using algorand cryptocurrency.<br> 
This project was created in just two days at &lt;/salt&gt; during "Hackdays". It is something I'd like to develop further on in the future.

## Description:
Qwitter requires users to create an account and login to be apart of the community. Once the user is logged in they can post their thoughts, jokes or questions in the master feed where other users can like or dislike their post. Every user can like and dislike any post as many times as they want, <b>this is not a bug, it's a feature.</b> The infinite liking feature inflates the concept of likes and relieves the pressure of social media. Users can click on a post to view it and write a reply to it and view other users comments. At the top of the page there is a header with three buttons. The Qwitter button takes you to the master feed. The search bar lets you search for other users and click on their profile to view their posts. The profile button takes you to your profile where you can log out, buy premium and view your own posts.

## Technologies used:
### Backend:
- Dotnet webapi
- Entity Framework Core
- Algorand
### Frontend:
- React
- Material UI
### Database:
- Microsoft Sql Server
- Docker

## Future improvements:
- Send and recieve friend requests.
- Message friends.
    - End to end encrypted communication.
- Tag users in posts (Hey @alex How are you?).
- Delete and edit posts.
- Upload images or videos.
- Improved security.
- Reset password.
- Email verification.
- Change username.
- Multiple feeds. (Master, Hot, Trending, Friends)
- Deployment


## Screenshots:
- ### The master feed:
![The master feed](/img/home.PNG?raw=true "The master feed")
- ### Post details and comment section: (A qweet?)
![A post](/img/comments.PNG?raw=true "A post")
- ### Your profile page:
![Your profile](/img/profile.PNG?raw=true "Your profile")
- ### Buying premium:
![Buying premium](/img/buy_premium.PNG?raw=true "Buying premium")
- ### The search feature for profiles:
![Search for profiles](/img/users.PNG?raw=true "Search for profiles")


Post:   /login
Post:   /register

Get:    /user/me
Patch:  /user/bio
Patch:  /user/username

Post:   /posts
Get:    /posts/{username}
Get:    /posts/id/{postId}
Post:   /posts/id/{postId}/like
Post:   /posts/id/{postId}/dislike
