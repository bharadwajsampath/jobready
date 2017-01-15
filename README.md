# JOBREADY WORKER

This is a .Net Library which connects to the Jobready Student Management system through the web API.


Services Included are :

## Course Service
## Enrolment Service
## Event Service

### GetAllEventsForToday
Gets all the events which are scheduled for Today (Current date).

### GetEvent
Parameters: eventId

Gets the event details based on the eventId

### GetAllInviteesForTheEvent
Parameters: courseId, eventId

Gets all the parties invited for that particular course and event.

### GetEventAttendees
Parameters: courseId, eventId

Gets all the attendees in that course-event.

### MarkAttendance
Parameters: courseNumber, eventId and Attendee Object.

Marks the attendance for the party.

## Party Service

### GET 
Parameters : PartyID

Gets the party object which includes all the details for the party.

### GetFileNotes
Parameters:partyID

Gets all the fileNotes for the party



