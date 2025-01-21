# Random Chat

Random Chat is a desktop application where users can find friends and make connections by talking to other online users.
Users can count on anonymity since messages aren't stored on the web server and they don't have to create a profile to communicate.

## Tech Stack

- .NET 8.0
- C# 13
- WPF and XAML

## Technical Details

- Users send TCP packets to a web server to establish a connection and exchange messages
- Web Server pairs them with another user on the waitlist and keeps that connection open
- Messages from one user are just forwarded to the paired user and they exist while chat between them exists
