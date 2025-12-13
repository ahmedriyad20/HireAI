# HireAI - Technical Documentation

**Version:** 1.0  
**Last Updated:** December 2025

---

## Table of Contents

- [Overview](#overview)
- [System Architecture](#system-architecture)
- [Technology Stack](#technology-stack)
- [Installation Guide](#installation-guide)
- [Project Structure](#project-structure)
- [API Reference](#api-reference)
- [Database Schema](#database-schema)
- [AI Integration](#ai-integration)
- [Real-time Notifications](#real-time-notifications)
- [Payment Processing](#payment-processing)
- [Security](#security)
- [Data Flow Diagrams](#data-flow-diagrams)
- [Testing](#testing)
- [Deployment](#deployment)
- [Roadmap](#roadmap)

---

## Overview

HireAI is a full-stack recruitment management platform that leverages artificial intelligence to automate technical assessment generation. The system enables HR professionals to manage job postings and applications while providing applicants with a streamlined application experience.

### Core Capabilities

**For HR Users:**
- Job posting management with custom exam parameters
- Application tracking and candidate evaluation
- AI-powered technical assessment generation
- Real-time application notifications
- Analytics dashboard

**For Applicants:**
- Job search and filtering
- One-click application submission
- AI-generated mock exams for practice
- Resume upload to cloud storage
- Real-time status notifications

**Platform Features:**
- JWT-based authentication with role-based access control
- Google Gemini AI integration for exam question generation
- SignalR for real-time notifications
- Stripe payment processing for premium features
- AWS S3 for resume storage

---

## System Architecture

### High-Level Architecture

```
┌─────────────────────────────────────────────────────────────┐
│                     Client Layer (Angular)                   │
│                    http://localhost:4200                     │
└─────────────────────────────────────────────────────────────┘
                            │
                            ▼
┌─────────────────────────────────────────────────────────────┐
│                  API Gateway & Middleware                    │
│         JWT Auth │ CORS │ SignalR Hubs │ Swagger            │
└─────────────────────────────────────────────────────────────┘
                            │
                            ▼
┌─────────────────────────────────────────────────────────────┐
│              Business Logic Layer (Services)                 │
│    ExamService │ GeminiService │ PaymentService │ etc.      │
└─────────────────────────────────────────────────────────────┘
                            │
                            ▼
┌─────────────────────────────────────────────────────────────┐
│           Data Access Layer (Repositories + EF Core)         │
│              Generic Repository Pattern + Unit of Work       │
└─────────────────────────────────────────────────────────────┘
                            │
                            ▼
┌─────────────────────────────────────────────────────────────┐
│                    External Services                         │
│  SQL Server │ Gemini AI │ AWS S3 │ Stripe │ SignalR         │
└─────────────────────────────────────────────────────────────┘
```

### Backend Architecture

```
HireAI/
├── HireAI.API/                 # Presentation Layer
│   ├── Controllers/            # REST API endpoints
│   ├── Extensions/             # DI service registration
│   └── Program.cs              # Application bootstrap
│
├── HireAI.Service/             # Business Logic Layer
│   ├── Services/               # Business logic implementation
│   ├── Interfaces/             # Service contracts
│   └── Mapping/                # AutoMapper profiles
│
├── HireAI.Infrastructure/      # Data Access Layer
│   ├── Context/                # EF Core DbContext
│   ├── Repositories/           # Repository implementations
│   └── GenericBase/            # Generic repository pattern
│
├── HireAI.Data/                # Data Models
│   ├── Models/                 # Entity models
│   └── Helpers/                # DTOs and utilities
│
└── HireAI.Core/                # Shared Resources
    └── Shared/                 # Common utilities
```

### Frontend Architecture

```
hire-ai-front/
├── src/app/
│   ├── core/
│   │   ├── guards/             # Route guards (auth)
│   │   ├── interceptors/       # HTTP interceptors (JWT)
│   │   ├── models/             # TypeScript interfaces
│   │   └── services/           # API communication services
│   │
│   ├── features/
│   │   ├── auth/               # Login, registration
│   │   ├── hr/                 # HR-specific features
│   │   ├── applicant/          # Applicant-specific features
│   │   └── landing/            # Public landing page
│   │
│   ├── layouts/
│   │   ├── public-layout/      # Public pages wrapper
│   │   └── dashboard-layout/   # Authenticated pages wrapper
│   │
│   └── shared/                 # Reusable components
```

---

## Technology Stack

### Backend Stack

| Component | Technology | Version | Purpose |
|-----------|------------|---------|---------|
| Framework | ASP.NET Core | 9.0 | Web API framework |
| ORM | Entity Framework Core | 9.0 | Database operations |
| Database | SQL Server | 2019+ | Data persistence |
| Authentication | JWT Bearer | - | Stateless authentication |
| Mapping | AutoMapper | - | DTO transformations |
| AI Service | Google Gemini API | 2.5 Flash | Question generation |
| Real-time | SignalR | - | WebSocket notifications |
| Payments | Stripe API | - | Payment processing |
| Storage | AWS S3 | - | File storage |
| Documentation | Swagger/OpenAPI | - | API documentation |

### Frontend Stack

| Component | Technology | Version | Purpose |
|-----------|------------|---------|---------|
| Framework | Angular | 20.3 | SPA framework |
| Language | TypeScript | 5.9 | Type-safe development |
| Reactive | RxJS | 7.8 | Async operations |
| UI Library | Bootstrap | 5.3 | Component styling |
| Charts | Chart.js | 4.5 | Data visualization |
| Icons | Bootstrap Icons | 1.13 | Icon library |

---

## Installation Guide

### Prerequisites

- .NET 9.0 SDK
- Node.js 20.x or higher
- SQL Server 2019 or higher
- Git

### Backend Setup

**Step 1: Clone Repository**
```bash
git clone https://github.com/ahmedgndy/HireAI.git
cd HireAI
```

**Step 2: Configure Database**

Edit `HireAI.API/appsettings.json`:
```json
{
  "ConnectionStrings": {
    "HireAiDB": "Server=YOUR_SERVER;Database=HireAI;Trusted_Connection=True;TrustServerCertificate=True;"
  }
}
```

**Step 3: Configure External Services**

Add API keys to `appsettings.json`:
```json
{
  "Gemini": {
    "ApiKey": "YOUR_GEMINI_API_KEY"
  },
  "AWS": {
    "AccessKey": "YOUR_AWS_ACCESS_KEY",
    "SecretKey": "YOUR_AWS_SECRET_KEY",
    "Region": "us-east-1",
    "BucketName": "YOUR_BUCKET_NAME"
  },
  "Stripe": {
    "SecretKey": "YOUR_STRIPE_SECRET_KEY",
    "PublishableKey": "YOUR_STRIPE_PUBLISHABLE_KEY"
  },
  "JWT": {
    "SecurityKey": "YOUR_SECRET_KEY_MINIMUM_32_CHARACTERS",
    "IssuerIP": "http://localhost:5290",
    "AudienceIP": "http://localhost:4200"
  }
}
```

**Step 4: Restore Dependencies**
```bash
dotnet restore
```

**Step 5: Apply Database Migrations**
```bash
dotnet ef database update --project HireAI.Infrastructure --startup-project HireAI.API
```

**Step 6: Run API**
```bash
cd HireAI.API
dotnet run
```

API will be available at: `http://localhost:5290`  
Swagger UI: `http://localhost:5290/swagger`

### Frontend Setup

**Step 1: Navigate to Frontend Directory**
```bash
cd HireAi_Angular-main
```

**Step 2: Install Dependencies**
```bash
npm install
```

**Step 3: Start Development Server**
```bash
npm start
```

Application will be available at: `http://localhost:4200`

### Default Test Accounts

The database seeder creates default accounts:

| Role | Email | Password |
|------|-------|----------|
| HR | hr@hireai.com | Password123! |
| Applicant | applicant@hireai.com | Password123! |

---

## Project Structure

### Backend Layers

**1. HireAI.API - Presentation Layer**
- RESTful API controllers
- Middleware configuration
- Dependency injection setup
- Authentication and authorization

**2. HireAI.Service - Business Logic Layer**
- Business logic implementation
- Service interfaces and implementations
- AutoMapper configuration
- Validation logic

**3. HireAI.Infrastructure - Data Access Layer**
- Entity Framework Core context
- Repository pattern implementation
- Database migrations
- Data seeding

**4. HireAI.Data - Data Models**
- Entity models
- Data Transfer Objects (DTOs)
- Enumerations
- Helper classes

**5. HireAI.Core - Core Utilities**
- Shared resources
- Common utilities
- Constants and configurations

---

## API Reference

### Authentication Endpoints

#### Register HR User

**Endpoint:** `POST /api/Account/register-hr`

**Headers:**
```
Content-Type: application/json
```

**Request Body:**
```json
{
  "email": "hr@example.com",
  "password": "SecurePassword123!",
  "companyName": "Tech Corp",
  "firstName": "John",
  "lastName": "Doe"
}
```

**Success Response (200 OK):**
```json
{
  "userId": "abc123",
  "email": "hr@example.com",
  "role": "HR"
}
```

**Error Response (400 Bad Request):**
```json
{
  "errors": {
    "Email": ["Email is already registered"],
    "Password": ["Password must contain uppercase, lowercase, and number"]
  }
}
```

---

#### Register Applicant

**Endpoint:** `POST /api/Account/register-applicant`

**Headers:**
```
Content-Type: application/json
```

**Request Body:**
```json
{
  "email": "applicant@example.com",
  "password": "SecurePassword123!",
  "firstName": "Jane",
  "lastName": "Smith"
}
```

**Success Response (200 OK):**
```json
{
  "userId": "def456",
  "email": "applicant@example.com",
  "role": "Applicant"
}
```

**Error Response (400 Bad Request):**
```json
{
  "errors": {
    "Email": ["Invalid email format"]
  }
}
```

---

#### Login

**Endpoint:** `POST /api/Account/login`

**Headers:**
```
Content-Type: application/json
```

**Request Body:**
```json
{
  "email": "user@example.com",
  "password": "SecurePassword123!"
}
```

**Success Response (200 OK):**
```json
{
  "token": "eyJhbGciOiJIUzI1NiIsInR5cCI6IkpXVCJ9.eyJzdWIiOiIxMjM0NTY3ODkwIiwibmFtZSI6IkpvaG4gRG9lIiwiaWF0IjoxNTE2MjM5MDIyfQ.SflKxwRJSMeKKF2QT4fwpMeJf36POk6yJV_adQssw5c",
  "expiration": "2024-12-11T14:00:00Z",
  "role": "HR",
  "userId": "abc123"
}
```

**Error Response (401 Unauthorized):**
```json
{
  "error": "Invalid email or password"
}
```

---

### Job Management Endpoints

#### Create Job Post

**Endpoint:** `POST /api/JobPost`

**Headers:**
```
Authorization: Bearer {token}
Content-Type: application/json
```

**Request Body:**
```json
{
  "title": "Senior .NET Developer",
  "description": "We are looking for an experienced .NET developer with expertise in C#, ASP.NET Core, Entity Framework, and SQL Server. The candidate should have strong knowledge of design patterns, SOLID principles, and microservices architecture.",
  "companyName": "Tech Corp",
  "experienceLevel": "Senior",
  "examDurationMinutes": 45,
  "location": "Remote",
  "salary": "$100,000 - $150,000"
}
```

**Success Response (201 Created):**
```json
{
  "id": 1,
  "title": "Senior .NET Developer",
  "companyName": "Tech Corp",
  "createdAt": "2024-12-10T10:00:00Z"
}
```

**Error Response (400 Bad Request):**
```json
{
  "errors": {
    "Title": ["Title is required"],
    "Description": ["Description must be at least 50 characters"]
  }
}
```

---

#### Get All Jobs

**Endpoint:** `GET /api/AvailableJobs`

**Headers:**
```
Authorization: Bearer {token}
```

**Query Parameters:**
```
?page=1&pageSize=10&search=developer
```

**Success Response (200 OK):**
```json
{
  "data": [
    {
      "id": 1,
      "title": "Senior .NET Developer",
      "companyName": "Tech Corp",
      "location": "Remote",
      "experienceLevel": "Senior",
      "createdAt": "2024-12-10T10:00:00Z"
    }
  ],
  "totalCount": 50,
  "page": 1,
  "pageSize": 10
}
```

**Error Response (401 Unauthorized):**
```json
{
  "error": "Authentication required"
}
```

---

#### Get Job Details

**Endpoint:** `GET /api/JobPost/{jobId}`

**Headers:**
```
Authorization: Bearer {token}
```

**Success Response (200 OK):**
```json
{
  "id": 1,
  "title": "Senior .NET Developer",
  "description": "We are looking for an experienced .NET developer...",
  "companyName": "Tech Corp",
  "experienceLevel": "Senior",
  "examDurationMinutes": 45,
  "location": "Remote",
  "salary": "$100,000 - $150,000",
  "createdAt": "2024-12-10T10:00:00Z",
  "applicationsCount": 25
}
```

**Error Response (404 Not Found):**
```json
{
  "error": "Job post not found"
}
```

---

#### Update Job Post

**Endpoint:** `PUT /api/JobPost/{jobId}`

**Headers:**
```
Authorization: Bearer {token}
Content-Type: application/json
```

**Request Body:**
```json
{
  "title": "Senior .NET Developer (Updated)",
  "description": "Updated description...",
  "salary": "$110,000 - $160,000"
}
```

**Success Response (200 OK):**
```json
{
  "id": 1,
  "message": "Job post updated successfully"
}
```

**Error Response (403 Forbidden):**
```json
{
  "error": "You do not have permission to update this job post"
}
```

---

#### Delete Job Post

**Endpoint:** `DELETE /api/JobPost/{jobId}`

**Headers:**
```
Authorization: Bearer {token}
```

**Success Response (200 OK):**
```json
{
  "message": "Job post deleted successfully"
}
```

**Error Response (404 Not Found):**
```json
{
  "error": "Job post not found"
}
```

---

### Application Endpoints

#### Submit Application

**Endpoint:** `POST /api/Application`

**Headers:**
```
Authorization: Bearer {token}
Content-Type: application/json
```

**Request Body:**
```json
{
  "jobId": 1,
  "applicantId": 5,
  "coverLetter": "I am excited to apply for this position..."
}
```

**Success Response (201 Created):**
```json
{
  "applicationId": 10,
  "jobId": 1,
  "status": "Pending",
  "submittedAt": "2024-12-10T11:00:00Z"
}
```

**Error Response (400 Bad Request):**
```json
{
  "error": "You have already applied to this job"
}
```

---

#### Get Applicant Applications

**Endpoint:** `GET /api/Application/applicant/{applicantId}`

**Headers:**
```
Authorization: Bearer {token}
```

**Success Response (200 OK):**
```json
{
  "applications": [
    {
      "id": 10,
      "jobTitle": "Senior .NET Developer",
      "companyName": "Tech Corp",
      "status": "Pending",
      "submittedAt": "2024-12-10T11:00:00Z"
    }
  ]
}
```

**Error Response (403 Forbidden):**
```json
{
  "error": "Access denied"
}
```

---

#### Get Job Applications (HR)

**Endpoint:** `GET /api/Application/job/{jobId}`

**Headers:**
```
Authorization: Bearer {token}
```

**Success Response (200 OK):**
```json
{
  "applications": [
    {
      "id": 10,
      "applicantName": "Jane Smith",
      "applicantEmail": "jane@example.com",
      "status": "Pending",
      "submittedAt": "2024-12-10T11:00:00Z",
      "resumeUrl": "https://s3.amazonaws.com/bucket/resume.pdf"
    }
  ]
}
```

**Error Response (404 Not Found):**
```json
{
  "error": "Job post not found"
}
```

---

#### Update Application Status

**Endpoint:** `PUT /api/Application/{applicationId}/status`

**Headers:**
```
Authorization: Bearer {token}
Content-Type: application/json
```

**Request Body:**
```json
{
  "status": "Accepted"
}
```

**Success Response (200 OK):**
```json
{
  "applicationId": 10,
  "status": "Accepted",
  "updatedAt": "2024-12-10T12:00:00Z"
}
```

**Error Response (400 Bad Request):**
```json
{
  "error": "Invalid status value"
}
```

---

### AI Exam Generation Endpoints

#### Create Job Exam with AI

**Endpoint:** `POST /api/Exam/job-exam/{applicationId}`

**Headers:**
```
Authorization: Bearer {token}
```

**Success Response (200 OK):**
```json
{
  "message": "Job exam created successfully with AI-generated questions",
  "examId": 50,
  "questionCount": 10,
  "questions": [
    {
      "id": 101,
      "questionText": "What is the main purpose of dependency injection in .NET?",
      "questionNumber": 1,
      "examId": 50
    },
    {
      "id": 102,
      "questionText": "Which design pattern is commonly used for creating objects?",
      "questionNumber": 2,
      "examId": 50
    }
  ]
}
```

**Error Response (400 Bad Request):**
```json
{
  "error": "Application not found"
}
```

**Error Response (400 Bad Request):**
```json
{
  "error": "Job post does not have a description"
}
```

---

#### Create Mock Exam with AI

**Endpoint:** `POST /api/Exam/mock-exam/{examId}`

**Headers:**
```
Authorization: Bearer {token}
```

**Success Response (200 OK):**
```json
{
  "message": "Mock exam questions generated successfully",
  "examId": 5,
  "questionCount": 10,
  "questions": [
    {
      "id": 201,
      "questionText": "Which data structure uses LIFO (Last In First Out) principle?",
      "questionNumber": 1,
      "examId": 5
    }
  ]
}
```

**Error Response (400 Bad Request):**
```json
{
  "error": "Exam not found"
}
```

**Error Response (400 Bad Request):**
```json
{
  "error": "Exam does not have a description"
}
```

---

#### Get Exam Details

**Endpoint:** `GET /api/Exam/{examId}`

**Headers:**
```
Authorization: Bearer {token}
```

**Success Response (200 OK):**
```json
{
  "id": 50,
  "examName": "Senior .NET Developer Assessment",
  "examLevel": "Advanced",
  "numberOfQuestions": 10,
  "durationInMinutes": 45,
  "isAi": true,
  "createdAt": "2024-12-10T13:00:00Z",
  "questions": [
    {
      "id": 101,
      "questionText": "What is dependency injection?",
      "questionNumber": 1
    }
  ]
}
```

**Error Response (404 Not Found):**
```json
{
  "error": "Exam not found"
}
```

---

### Dashboard Endpoints

#### HR Dashboard

**Endpoint:** `GET /api/HRDashboard`

**Headers:**
```
Authorization: Bearer {token}
```

**Success Response (200 OK):**
```json
{
  "totalJobs": 15,
  "activeJobs": 10,
  "totalApplications": 150,
  "pendingApplications": 45,
  "acceptedApplications": 30,
  "rejectedApplications": 75
}
```

**Error Response (403 Forbidden):**
```json
{
  "error": "HR role required"
}
```

---

#### Applicant Dashboard

**Endpoint:** `GET /api/ApplicantDashboard`

**Headers:**
```
Authorization: Bearer {token}
```

**Success Response (200 OK):**
```json
{
  "totalApplications": 5,
  "pendingApplications": 2,
  "acceptedApplications": 2,
  "rejectedApplications": 1,
  "completedExams": 3
}
```

**Error Response (403 Forbidden):**
```json
{
  "error": "Applicant role required"
}
```

---

### File Upload Endpoints

#### Upload Resume

**Endpoint:** `POST /api/Applicant/upload-resume`

**Headers:**
```
Authorization: Bearer {token}
Content-Type: multipart/form-data
```

**Request Body:**
```
file: [binary data]
```

**Success Response (200 OK):**
```json
{
  "fileUrl": "https://bucket.s3.amazonaws.com/resumes/abc123-resume.pdf",
  "fileName": "resume.pdf",
  "uploadedAt": "2024-12-10T14:00:00Z"
}
```

**Error Response (400 Bad Request):**
```json
{
  "error": "Invalid file format. Only PDF files are allowed"
}
```

---

## Database Schema

### Entity Relationship Diagram

```
ApplicationUser (HR) 1──────────* JobPost
ApplicationUser (Applicant) 1──* Application
JobPost 1──────────────────────* Application
Application 1──────────────────1 Exam
Exam 1─────────────────────────* Question
Question 1─────────────────────* ApplicantResponse
```

### Core Entities

**ApplicationUser**
- Inherits from IdentityUser
- Fields: Id, Email, FirstName, LastName, Role (HR/Applicant), CompanyName (HR only), ResumeUrl (Applicant only)
- Relationships: JobPosts (HR), Applications (Applicant)

**JobPost**
- Fields: Id, Title, Description, CompanyName, ExperienceLevel, ExamDurationMinutes, Location, Salary, CreatedAt, HRId
- Relationships: Applications, HR User

**Application**
- Fields: Id, JobId, ApplicantId, Status, CoverLetter, SubmittedAt, ExamId
- Relationships: JobPost, Applicant, Exam

**Exam**
- Fields: Id, ExamName, ExamDescription, ExamLevel, NumberOfQuestions, DurationInMinutes, IsAi, ExamType (MockExam/HrExam), CreatedAt
- Relationships: Questions, Applications

**Question**
- Fields: Id, QuestionText, Choices (string array, 4 items), CorrectAnswerIndex (0-3), QuestionNumber, ExamId
- Relationships: Exam, ApplicantResponses

**ApplicantResponse**
- Fields: Id, QuestionId, ApplicantId, SelectedAnswerIndex, IsCorrect
- Relationships: Question, Applicant

---

## AI Integration

### Google Gemini 2.5 Flash Implementation

HireAI uses Google's Gemini 2.5 Flash model to automatically generate technical assessment questions based on job descriptions or exam topics.

### Service Implementation

**File:** `HireAI.Service/Services/GeminiService.cs`

**Key Methods:**
- `GenerateJobExamQuestionsAsync(string jobDescription)` - Generates questions for job-specific exams
- `GenerateMockExamQuestionsAsync(string examDescription)` - Generates questions for practice exams

### Configuration Parameters

```csharp
generationConfig = new
{
    temperature = 0.7,      // Balanced creativity and consistency
    maxOutputTokens = 4096, // Sufficient for 10 questions
    topP = 0.9,
    topK = 40
}
```

### Rate Limiting

To comply with Gemini API free tier limits (15 requests/minute):

```csharp
private const int MinMillisecondsBetweenRequests = 4000; // ~15 req/min
private static readonly SemaphoreSlim _rateLimiter = new SemaphoreSlim(1, 1);
```

Implementation enforces 4-second delays between consecutive requests.

### AI Prompt Template

```
Generate 10 multiple-choice questions (MCQ) for a {examType} based on the following description.

Description: {description}

Requirements:
1. Generate exactly 10 questions
2. Each question must have exactly 4 choices
3. Each question must have one correct answer
4. Questions should be relevant to the description
5. Difficulty should match the experience level
6. Index the correct answer (0-3)

Return ONLY a JSON response in this exact format:
{
  "questions": [
    {
      "questionText": "Question text here",
      "choices": ["Choice A", "Choice B", "Choice C", "Choice D"],
      "correctAnswerIndex": 0
    }
  ]
}
```

### Difficulty Mapping

Experience levels are automatically mapped to exam difficulty:

```csharp
ExamLevel = jobPost.ExperienceLevel switch
{
    enExperienceLevel.EntryLevel => enExamLevel.Beginner,
    enExperienceLevel.Junior => enExamLevel.Beginner,
    enExperienceLevel.MidLevel => enExamLevel.Intermediate,
    enExperienceLevel.Senior => enExamLevel.Advanced,
    enExperienceLevel.Lead => enExamLevel.Advanced,
    _ => enExamLevel.Intermediate
}
```

### Error Handling

The service handles:
- Safety filter violations
- Token limit exceeded errors
- JSON parsing failures
- Network connectivity issues
- Rate limit enforcement

### API Rate Limits

| Tier | Requests/Min | Requests/Day | Tokens/Min |
|------|--------------|--------------|------------|
| Free | 15 | 1,500 | 32,000 |

Current implementation automatically throttles to comply with free tier limits.

---

## Real-time Notifications

### SignalR Implementation

HireAI uses SignalR to provide real-time notifications for application status updates and system events.

### Hub Implementation

**File:** `HireAI.API/Hubs/NotificationHub.cs`

```csharp
public class NotificationHub : Hub
{
    public async Task SendNotification(string userId, string message)
    {
        await Clients.User(userId).SendAsync("ReceiveNotification", message);
    }
    
    public async Task NotifyApplicationStatusChange(string applicantId, string status)
    {
        await Clients.User(applicantId).SendAsync("ApplicationStatusChanged", status);
    }
    
    public async Task NotifyNewApplication(string hrId, string jobTitle)
    {
        await Clients.User(hrId).SendAsync("NewApplication", jobTitle);
    }
}
```

### Client Connection (Angular)

```typescript
import * as signalR from '@microsoft/signalr';

export class NotificationService {
  private hubConnection: signalR.HubConnection;
  
  startConnection() {
    this.hubConnection = new signalR.HubConnectionBuilder()
      .withUrl('http://localhost:5290/notificationHub', {
        accessTokenFactory: () => this.authService.getToken()
      })
      .build();
      
    this.hubConnection.start()
      .then(() => console.log('SignalR Connected'))
      .catch(err => console.error('SignalR Connection Error:', err));
  }
  
  addNotificationListener() {
    this.hubConnection.on('ReceiveNotification', (message) => {
      this.displayNotification(message);
    });
    
    this.hubConnection.on('ApplicationStatusChanged', (status) => {
      this.handleStatusChange(status);
    });
  }
}
```

### Notification Events

| Event Name | Trigger | Recipient | Payload |
|------------|---------|-----------|---------|
| NewApplication | Application submitted | HR User | Job title, applicant name |
| ApplicationStatusChanged | HR updates status | Applicant | New status |
| ExamInvitation | Exam assigned | Applicant | Exam details |
| SystemAlert | Admin action | All Users | Alert message |

---

## Payment Processing

### Stripe Integration

HireAI integrates Stripe for processing payments for premium features and subscriptions.

### Service Implementation

**File:** `HireAI.Service/Services/PaymentService.cs`

```csharp
public class PaymentService : IPaymentService
{
    private readonly StripeClient _stripeClient;
    
    public async Task<PaymentIntent> CreatePaymentIntent(decimal amount, string currency)
    {
        var options = new PaymentIntentCreateOptions
        {
            Amount = (long)(amount * 100), // Convert to cents
            Currency = currency,
            PaymentMethodTypes = new List<string> { "card" }
        };
        
        var service = new PaymentIntentService(_stripeClient);
        return await service.CreateAsync(options);
    }
    
    public async Task<Subscription> CreateSubscription(string customerId, string priceId)
    {
        var options = new SubscriptionCreateOptions
        {
            Customer = customerId,
            Items = new List<SubscriptionItemOptions>
            {
                new SubscriptionItemOptions { Price = priceId }
            }
        };
        
        var service = new SubscriptionService(_stripeClient);
        return await service.CreateAsync(options);
    }
}
```

### Payment Endpoints

**Create Payment Intent**

**Endpoint:** `POST /api/Payment/create-payment-intent`

**Headers:**
```
Authorization: Bearer {token}
Content-Type: application/json
```

**Request Body:**
```json
{
  "amount": 99.99,
  "currency": "usd",
  "description": "Premium Job Posting"
}
```

**Success Response (200 OK):**
```json
{
  "clientSecret": "pi_xxx_secret_xxx",
  "paymentIntentId": "pi_xxx"
}
```

**Error Response (400 Bad Request):**
```json
{
  "error": "Invalid amount"
}
```

---

**Create Subscription**

**Endpoint:** `POST /api/Payment/create-subscription`

**Headers:**
```
Authorization: Bearer {token}
Content-Type: application/json
```

**Request Body:**
```json
{
  "priceId": "price_xxx",
  "customerId": "cus_xxx"
}
```

**Success Response (200 OK):**
```json
{
  "subscriptionId": "sub_xxx",
  "status": "active",
  "currentPeriodEnd": "2025-01-10T00:00:00Z"
}
```

**Error Response (400 Bad Request):**
```json
{
  "error": "Invalid customer ID"
}
```

### Pricing Plans

| Plan | Price | Job Posts | Features |
|------|-------|-----------|----------|
| Free | $0/month | 5 | Basic analytics |
| Professional | $49/month | 25 | Advanced analytics, Priority support |
| Enterprise | $199/month | Unlimited | Custom branding, Dedicated support, API access |

### Webhook Handling

```csharp
[HttpPost("webhook")]
public async Task<IActionResult> StripeWebhook()
{
    var json = await new StreamReader(HttpContext.Request.Body).ReadToEndAsync();
    var stripeEvent = EventUtility.ConstructEvent(
        json, 
        Request.Headers["Stripe-Signature"], 
        _webhookSecret
    );
    
    switch (stripeEvent.Type)
    {
        case "payment_intent.succeeded":
            await HandlePaymentSuccess(stripeEvent.Data.Object as PaymentIntent);
            break;
        case "payment_intent.payment_failed":
            await HandlePaymentFailure(stripeEvent.Data.Object as PaymentIntent);
            break;
        case "customer.subscription.created":
            await HandleSubscriptionCreated(stripeEvent.Data.Object as Subscription);
            break;
        case "customer.subscription.deleted":
            await HandleSubscriptionCancelled(stripeEvent.Data.Object as Subscription);
            break;
    }
    
    return Ok();
}
```

---

## Security

### Authentication

**JWT Bearer Token Authentication**

Configuration in `Program.cs`:
```csharp
builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
    .AddJwtBearer(options =>
    {
        options.TokenValidationParameters = new TokenValidationParameters
        {
            ValidateIssuer = true,
            ValidateAudience = true,
            ValidateLifetime = true,
            ValidateIssuerSigningKey = true,
            ValidIssuer = configuration["JWT:IssuerIP"],
            ValidAudience = configuration["JWT:AudienceIP"],
            IssuerSigningKey = new SymmetricSecurityKey(
                Encoding.UTF8.GetBytes(configuration["JWT:SecurityKey"])
            )
        };
    });
```

### Authorization

**Role-Based Access Control (RBAC)**

Two primary roles:
- HR: Can create jobs, view applications, update application status
- Applicant: Can apply to jobs, take exams, view own applications

Example controller authorization:
```csharp
[Authorize(Roles = "HR")]
public class JobPostController : ControllerBase
{
    // HR-only endpoints
}

[Authorize(Roles = "Applicant")]
public class ApplicationController : ControllerBase
{
    // Applicant-only endpoints
}
```

### Data Protection

**Password Security**
- ASP.NET Core Identity with bcrypt hashing
- Minimum password requirements enforced
- Password reset functionality with secure tokens

**Data Encryption**
- HTTPS/TLS 1.2+ for data in transit
- Database encryption at rest (SQL Server TDE)
- Sensitive configuration in environment variables

**File Storage Security**
- AWS S3 private buckets
- Pre-signed URLs with expiration
- File type validation on upload

**API Security**
- CORS policies configured for allowed origins
- Rate limiting to prevent abuse
- Input validation on all endpoints
- SQL injection prevention via parameterized queries (EF Core)

### CORS Configuration

```csharp
builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowAll", builder =>
    {
        builder.AllowAnyOrigin()
               .AllowAnyMethod()
               .AllowAnyHeader();
    });
    
    options.AddPolicy("ProductionPolicy", builder =>
    {
        builder.WithOrigins("https://hireai.com")
               .AllowAnyMethod()
               .AllowAnyHeader()
               .AllowCredentials();
    });
});
```

---

## Data Flow Diagrams

### User Login Flow

```
┌─────────┐
│ Client  │
└────┬────┘
     │ 1. POST /api/Account/login
     │    {email, password}
     ▼
┌─────────────────┐
│ API Controller  │
└────┬────────────┘
     │ 2. Validate credentials
     ▼
┌─────────────────┐
│ Identity Service│
└────┬────────────┘
     │ 3. Check user in DB
     ▼
┌─────────────────┐
│   Database      │
└────┬────────────┘
     │ 4. User found
     ▼
┌─────────────────┐
│  JWT Service    │
└────┬────────────┘
     │ 5. Generate token
     ▼
┌─────────┐
│ Client  │ 6. Return {token, role, userId}
└─────────┘
```

### AI Exam Generation Flow

```
┌─────────┐
│   HR    │
└────┬────┘
     │ 1. POST /api/Exam/job-exam/{applicationId}
     ▼
┌─────────────────┐
│ ExamController  │
└────┬────────────┘
     │ 2. Get application & job details
     ▼
┌─────────────────┐
│  ExamService    │
└────┬────────────┘
     │ 3. Extract job description
     ▼
┌─────────────────┐
│ GeminiService   │
└────┬────────────┘
     │ 4. Send prompt to Gemini API
     ▼
┌─────────────────┐
│  Gemini API     │
└────┬────────────┘
     │ 5. Return 10 questions (JSON)
     ▼
┌─────────────────┐
│ GeminiService   │
└────┬────────────┘
     │ 6. Parse JSON response
     ▼
┌─────────────────┐
│  ExamService    │
└────┬────────────┘
     │ 7. Create Exam entity
     │ 8. Create 10 Question entities
     ▼
┌─────────────────┐
│   Database      │
└────┬────────────┘
     │ 9. Save exam & questions
     ▼
┌─────────┐
│   HR    │ 10. Return {examId, questions}
└─────────┘
```

### Real-time Notification Flow

```
┌─────────┐
│   HR    │
└────┬────┘
     │ 1. PUT /api/Application/{id}/status
     │    {status: "Accepted"}
     ▼
┌──────────────────┐
│ApplicationService│
└────┬─────────────┘
     │ 2. Update application status
     ▼
┌─────────────────┐
│   Database      │
└────┬────────────┘
     │ 3. Status updated
     ▼
┌──────────────────┐
│NotificationService│
└────┬─────────────┘
     │ 4. Trigger notification
     ▼
┌─────────────────┐
│  SignalR Hub    │
└────┬────────────┘
     │ 5. Send to applicant's connection
     ▼
┌─────────────────┐
│  Applicant      │ 6. Receive real-time update
│  (Browser)      │    "Your application was accepted"
└─────────────────┘
```

### Payment Processing Flow

```
┌─────────┐
│   HR    │
└────┬────┘
     │ 1. POST /api/Payment/create-payment-intent
     │    {amount: 49.99, currency: "usd"}
     ▼
┌─────────────────┐
│PaymentController│
└────┬────────────┘
     │ 2. Create payment intent
     ▼
┌─────────────────┐
│ PaymentService  │
└────┬────────────┘
     │ 3. Call Stripe API
     ▼
┌─────────────────┐
│   Stripe API    │
└────┬────────────┘
     │ 4. Return client secret
     ▼
┌─────────────────┐
│PaymentController│
└────┬────────────┘
     │ 5. Return {clientSecret, paymentIntentId}
     ▼
┌─────────┐
│   HR    │ 6. Use client secret for payment
│ (Client)│
└────┬────┘
     │ 7. Complete payment with Stripe.js
     ▼
┌─────────────────┐
│   Stripe        │
└────┬────────────┘
     │ 8. Send webhook: payment_intent.succeeded
     ▼
┌─────────────────┐
│ Webhook Handler │
└────┬────────────┘
     │ 9. Update subscription in DB
     ▼
┌─────────────────┐
│   Database      │
└─────────────────┘
```

---

## Testing

### Backend Testing

**Run All Tests:**
```bash
dotnet test
```

**Run with Code Coverage:**
```bash
dotnet test /p:CollectCoverage=true /p:CoverageReportFormat=opencover
```

**Run Specific Test Project:**
```bash
dotnet test HireAI.Tests/HireAI.Tests.csproj
```

### Frontend Testing

**Run Unit Tests:**
```bash
npm test
```

**Run Tests in Watch Mode:**
```bash
npm test -- --watch
```

**Run End-to-End Tests:**
```bash
npm run e2e
```

**Generate Coverage Report:**
```bash
npm test -- --coverage
```

### Manual API Testing

Use Swagger UI at `http://localhost:5290/swagger` for interactive API testing.

Alternatively, use the provided Postman collection or cURL commands documented in each endpoint section.

---

## Deployment

### Production Checklist

**Backend:**
- [ ] Update connection strings for production database
- [ ] Configure production API keys (Gemini, AWS, Stripe)
- [ ] Set strong JWT secret key (minimum 32 characters)
- [ ] Enable HTTPS enforcement
- [ ] Configure production CORS policy
- [ ] Set up database backups
- [ ] Configure logging and monitoring
- [ ] Apply database migrations

**Frontend:**
- [ ] Update API base URL to production endpoint
- [ ] Build production bundle: `npm run build`
- [ ] Configure CDN for static assets
- [ ] Enable production mode
- [ ] Set up error tracking (e.g., Sentry)

**Infrastructure:**
- [ ] Configure SSL certificates
- [ ] Set up load balancer (if needed)
- [ ] Configure firewall rules
- [ ] Set up monitoring and alerts
- [ ] Configure backup strategy

### Environment Variables

**Backend (appsettings.Production.json):**
```json
{
  "ConnectionStrings": {
    "HireAiDB": "PRODUCTION_CONNECTION_STRING"
  },
  "Gemini": {
    "ApiKey": "PRODUCTION_GEMINI_KEY"
  },
  "AWS": {
    "AccessKey": "PRODUCTION_AWS_ACCESS_KEY",
    "SecretKey": "PRODUCTION_AWS_SECRET_KEY",
    "Region": "us-east-1",
    "BucketName": "hireai-production"
  },
  "Stripe": {
    "SecretKey": "PRODUCTION_STRIPE_SECRET_KEY",
    "PublishableKey": "PRODUCTION_STRIPE_PUBLISHABLE_KEY"
  },
  "JWT": {
    "SecurityKey": "PRODUCTION_JWT_SECRET_KEY_MINIMUM_32_CHARS",
    "IssuerIP": "https://api.hireai.com",
    "AudienceIP": "https://hireai.com"
  }
}
```

---

## Roadmap

### Short-term (Q1 2025)

- Enhanced analytics dashboard with custom date ranges
- Email notifications for application updates
- Bulk application management for HR
- Advanced search filters for job listings
- Mobile-responsive improvements

### Mid-term (Q2-Q3 2025)

- Video interview integration
- AI-powered resume parsing and matching
- Automated interview scheduling
- Multi-language support (Spanish, French, German)
- Advanced reporting and export features
- Integration with LinkedIn for job posting

### Long-term (Q4 2025 and beyond)

- Native mobile applications (iOS and Android)
- Machine learning for candidate ranking
- Custom branding for enterprise clients
- API access for third-party integrations
- Advanced security features (2FA, SSO)
- Blockchain-based credential verification

---

## Support

For technical support or questions:

**Email:** support@hireai.com  
**GitHub Issues:** https://github.com/ahmedgndy/HireAI/issues  
**Documentation:** https://docs.hireai.com

---

**HireAI Technical Documentation v1.0**  
**Last Updated:** December 2025
