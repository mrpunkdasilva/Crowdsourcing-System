# 05-UML Diagrams

UML (Unified Modeling Language) diagrams are essential tools for visualizing, specifying, constructing, and documenting the artifacts of a software system. For the CIS API, key UML diagrams help in understanding the system's behavior, structure, and interactions.

## Use Case Diagram

A Use Case Diagram illustrates the high-level functional requirements of the system and how different external actors interact with it. This diagram provides a bird's-eye view of the services the CIS API offers.

```plantuml
@startuml
!theme vibrant

left to right direction

actor "Authenticated User" as User
actor "API Client" as Client

rectangle "CIS API" {
    usecase "Manage Topics" as Topics
    usecase "Manage Ideas" as Ideas
    usecase "Manage Votes" as Votes
    usecase "Simulate API Usage" as Simulation
}

User --> Topics
User --> Ideas
User --> Votes

Client --> Simulation

note right of Topics
  Includes:
  - Create Topic
  - View Topics
end note

note right of Ideas
  Includes:
  - Create Idea
  - View Ideas
end note

note right of Votes
  Includes:
  - Vote on Idea
  - Cancel Vote
end note

Simulation ..> Topics : <<includes>>
Simulation ..> Ideas : <<includes>>
Simulation ..> Votes : <<includes>>

@enduml
```

## Sequence Diagrams

Sequence Diagrams illustrate the order of interactions between objects for a specific scenario. They are invaluable for detailing the step-by-step logic of an operation.

### Scenario: Creating a New Topic

This diagram shows the process of an authenticated user creating a new topic.

```plantuml
@startuml
!theme vibrant
title "Sequence Diagram: Creating a New Topic"

actor "User" as User
participant "CIS API" as CisApi
participant "User API" as UserApi
database "Database" as DB

autonumber

User -> CisApi: POST /topics (request with auth token, topic data)
activate CisApi

CisApi -> UserApi: Verify Token(authToken)
activate UserApi
UserApi --> CisApi: Token Valid
deactivate UserApi

CisApi -> DB: INSERT INTO topics (title, description)
activate DB
DB --> CisApi: New Topic Record (with ID)
deactivate DB

CisApi --> User: 201 Created (response with new topic object)
deactivate CisApi

@enduml
```

### Scenario: Voting on an Idea

This diagram details the logic for a user voting on an idea, including validation steps.

```plantuml
@startuml
!theme vibrant
title "Sequence Diagram: Voting on an Idea"

actor "User" as User
participant "CIS API" as CisApi
participant "User API" as UserApi
database "Database" as DB

autonumber

User -> CisApi: POST /ideas/{id}/vote (request with auth token)
activate CisApi

CisApi -> UserApi: Verify Token(authToken)
activate UserApi
UserApi --> CisApi: Token Valid (returns user ID)
deactivate UserApi

CisApi -> DB: Check for existing vote (userId, ideaId)
activate DB
DB --> CisApi: No existing vote found
deactivate DB

CisApi -> DB: INSERT INTO votes (userId, ideaId);\nUPDATE ideas SET vote_count = vote_count + 1;
activate DB
DB --> CisApi: Success
deactivate DB

CisApi --> User: 200 OK (response confirming vote)
deactivate CisApi

@enduml
```

### Scenario: Retrieving Ideas for a Topic

This diagram shows the simpler flow of retrieving data from the API.

```plantuml
@startuml
!theme vibrant
title "Sequence Diagram: Retrieving Ideas for a Topic"

actor "User" as User
participant "CIS API" as CisApi
database "Database" as DB

autonumber

User -> CisApi: GET /topics/{id}/ideas
activate CisApi

' Note: An authentication/authorization check would typically occur here,
' but is omitted for brevity as it is shown in other diagrams.

CisApi -> DB: SELECT * FROM ideas WHERE topic_id = {id}
activate DB
DB --> CisApi: List of Idea Records
deactivate DB

CisApi --> User: 200 OK (response with list of ideas)
deactivate CisApi

@enduml
```

## Class Diagrams (Optional, but Recommended)

Class Diagrams provide a static view of the system, illustrating the classes, their attributes, operations, and the relationships among them. For the CIS API, a Class Diagram could represent the core data models (Topic, Idea, Vote, User) and their relationships, providing a clear understanding of the API's internal structure.

*(Placeholder for Class Diagram image)*

These diagrams serve as a visual guide for developers, helping them to understand the system's design and behavior before diving into the code.
