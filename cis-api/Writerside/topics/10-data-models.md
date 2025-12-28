# 10-Data Models (Schemas)

This section describes the key data models (schemas) used within the CIS API. Understanding these models is crucial for interacting with the API, as they define the structure of the data exchanged in requests and responses.

## Topic

A `Topic` represents a category or subject under which ideas are proposed. Each topic has a unique identifier, a title, and a description.

| Field       | Type     | Description                                     | Example Value                                |
| :---------- | :------- | :---------------------------------------------- | :------------------------------------------- |
| `id`        | `string` | Unique identifier for the topic (UUID).         | `a1b2c3d4-e5f6-7890-1234-567890abcdef`       |
| `title`     | `string` | The title of the topic.                         | `"Sustainable Energy Solutions"`             |
| `description` | `string` | A brief description of what the topic is about. | `"Ideas for renewable energy and conservation."` |
| `createdAt` | `string` | Timestamp when the topic was created (ISO 8601). | `"2023-10-27T10:00:00Z"`                     |
| `updatedAt` | `string` | Timestamp when the topic was last updated (ISO 8601). | `"2023-10-27T10:00:00Z"`                     |

**Example JSON (Topic):**

```json
{
  "id": "a1b2c3d4-e5f6-7890-1234-567890abcdef",
  "title": "Sustainable Energy Solutions",
  "description": "Ideas for renewable energy and conservation.",
  "createdAt": "2023-10-27T10:00:00Z",
  "updatedAt": "2023-10-27T10:00:00Z"
}
```

## Idea

An `Idea` represents a specific suggestion or proposal submitted under a particular topic by a user.

| Field       | Type     | Description                                     | Example Value                                |
| :---------- | :------- | :---------------------------------------------- | :------------------------------------------- |
| `id`        | `string` | Unique identifier for the idea (UUID).          | `b2c3d4e5-f6a7-8901-2345-67890abcdef0`       |
| `topicId`   | `string` | The ID of the topic this idea belongs to.       | `a1b2c3d4-e5f6-7890-1234-567890abcdef`       |
| `userId`    | `string` | The ID of the user who proposed the idea.       | `user-123-abc`                               |
| `title`     | `string` | The title of the idea.                          | `"Solar Panels for Public Buildings"`        |
| `description` | `string` | A detailed description of the idea.             | `"Install solar panels on all municipal buildings to reduce energy consumption."` |
| `votesCount`| `integer`| The total number of votes received by the idea. | `150`                                        |
| `createdAt` | `string` | Timestamp when the idea was created (ISO 8601). | `"2023-10-27T11:30:00Z"`                     |
| `updatedAt` | `string` | Timestamp when the idea was last updated (ISO 8601). | `"2023-10-27T12:00:00Z"`                     |

**Example JSON (Idea):**

```json
{
  "id": "b2c3d4e5-f6a7-8901-2345-67890abcdef0",
  "topicId": "a1b2c3d4-e5f6-7890-1234-567890abcdef",
  "userId": "user-123-abc",
  "title": "Solar Panels for Public Buildings",
  "description": "Install solar panels on all municipal buildings to reduce energy consumption.",
  "votesCount": 150,
  "createdAt": "2023-10-27T11:30:00Z",
  "updatedAt": "2023-10-27T12:00:00Z"
}
```

## Vote

A `Vote` represents a user's endorsement or disapproval of an idea.

| Field       | Type     | Description                                     | Example Value                                |
| :---------- | :------- | :---------------------------------------------- | :------------------------------------------- |
| `id`        | `string` | Unique identifier for the vote (UUID).          | `c3d4e5f6-a7b8-9012-3456-7890abcdef01`       |
| `ideaId`    | `string` | The ID of the idea being voted on.              | `b2c3d4e5-f6a7-8901-2345-67890abcdef0`       |
| `userId`    | `string` | The ID of the user who cast the vote.           | `user-456-def`                               |
| `type`      | `string` | The type of vote (e.g., `"upvote"`, `"downvote"`). | `"upvote"`                                   |
| `createdAt` | `string` | Timestamp when the vote was cast (ISO 8601).    | `"2023-10-27T12:15:00Z"`                     |

**Example JSON (Vote):**

```json
{
  "id": "c3d4e5f6-a7b8-9012-3456-7890abcdef01",
  "ideaId": "b2c3d4e5-f6a7-8901-2345-67890abcdef0",
  "userId": "user-456-def",
  "type": "upvote",
  "createdAt": "2023-10-27T12:15:00Z"
}
```

## User (from Users API)

The `User` model is managed by the external Users API. The CIS API interacts with user identifiers (`userId`) but does not store or manage full user profiles. For detailed information on the User model, refer to the Users API documentation.

| Field       | Type     | Description                                     | Example Value                                |
| :---------- | :------- | :---------------------------------------------- | :------------------------------------------- |
| `id`        | `string` | Unique identifier for the user (UUID).          | `user-123-abc`                               |
| `name`      | `string` | The user's full name.                           | `"John Doe"`                                 |
| `login`     | `string` | The user's login username.                      | `"johndoe"`                                  |

*(Note: The `name` and `login` fields are typically retrieved from the Users API when needed for display purposes, but not directly managed by the CIS API.)*

For more detailed schema definitions, including validation rules and additional properties, please refer to the [OpenAPI Schemas](08-api-reference.md) section.