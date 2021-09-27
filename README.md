# AduwuJoseph-HotelBookingApi-Using-Hangfire-Stateless-Machine
## The Problem
Your Client is a Luxury Hotel. The management still employs manual processes to carry out day to day
activities. You have been commissioned to help the management perform better with technology solutions
choosing Desktop, Mobile, Tablet as their media.
You are to come up with a Hotel reservation system Backend API showing some of core functionality
below;
Room States { Free, Occupied, Reserved, Cleaned, Unavailable }
Client States { Registered, BookedRoom, Check-In, Check-Out }
Client
● Register customer (process)
● Send email to customer on successful registration
● Book/reserve room after reviewing the rooms
● Get email confirmation of room reserved
Admin
● Add rooms setup (process)
● Dashboards data; total rooms, occupancy, free rooms, revenue, check-ins and check-outs
● Which customer booked which room certain days and services consumed and how much he will
pay
The goal is to complete a working api for the Hotel Booking Api Application with workflow features based
on the states listed above.
You should consider workflow libraries like; stateless (https://github.com/dotnet-state-machine/stateless)
to achieve this task.
For example On successful BookedRoom and Check-Out states; email should be fired as additional
action on the client’s workflow defined in the implementation.
Sending of mails and background jobs in the application will be handled by the Hangfire library.
Your delivery should show via api call or endpoints the histories of each rooms and clients in the hotel
MongoDB Database, .Net Core 3.1 - 5.0 and json output compatible rest api.
Knowledge of MongoDB Aggregation Framework will count as an advantage for you.
Your code should show separation of concerns; services, repository and applicable design
patterns for your implementation

## The Solution

The API is implemented in Asp.Net 5 web Api using MongoDB
* After a successful booking, a hangfire background job is fired to send an email to the customer
* Also after a customer checks out, an email is send to the customer using Hangfire.BackgroundJobs

The Stateless Machine workflow part is yet to be completed