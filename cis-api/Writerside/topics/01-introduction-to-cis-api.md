# 01-Introduction to CIS API

This guide focuses on the implementation of the Collaborative Ideation Solution (CIS) API, which is the core component developed during Phase 2 of the Capstone Project. It details the API's functionalities, architecture, and development conventions.

## Project Conventions

To ensure consistency and clarity throughout the project, the following conventions will be rigorously followed:

*   **Language**: The entire project, including documentation (wikis, specifications, etc.), source code, comments, and commit messages, will be developed and maintained entirely in English.
*   **Code Style**: The code style will follow the best practices and standards established for each language and framework used. Specific details will be defined and communicated by the teams.
*   **Documentation**: Documentation will be clear, concise, and aimed at junior developers, focusing on the *why* and *how* of implementations.

## Product Definition

The CIS API is the core component of the Collaborative Ideation Solution (CIS), also known as "Crowdsourced Ideation Solution", developed during Phase 2 of the Capstone Project. This API provides the functionalities for a collective creativity platform, allowing users to propose and vote on ideas.

Its main features, exposed via the API, include:

*   **Organization by Topic**: The API allows for ideas to be organized by topics, which can be arbitrary (e.g., "Recommended study habits", "Reduce global warming").
*   **Idea Proposal and Voting**: The API enables users to propose innovative ideas for these topics and to cast votes on the best proposals.

### How it Works

The CIS API facilitates the following interactions:

1.  **Topics Management**: The API allows users to create, view, and manage topics, which serve as categories for ideas (e.g., "Recommended study habits", "Reduce global warming", etc.).
2.  **Idea and Vote Management**: Through the API, users can propose innovative ideas within specific topics and cast votes (positive or negative) on existing ideas to classify them.

The overall goal of the CIS API is to provide the programmatic interface for a collaborative environment where ideas can be generated and refined collectively.