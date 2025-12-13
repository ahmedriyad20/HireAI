# HireAI - AI-Powered Recruitment Platform

<div align="center">

![HireAI Logo](https://img.shields.io/badge/HireAI-Recruitment%20Platform-blue?style=for-the-badge)
![.NET](https://img.shields.io/badge/.NET-9.0-512BD4?style=for-the-badge&logo=dotnet)
![Angular](https://img.shields.io/badge/Angular-20.3-DD0031?style=for-the-badge&logo=angular)

**A modern, full-stack recruitment platform powered by AI for intelligent exam generation and streamlined hiring processes.**

[Features](#features) • [Tech Stack](#tech-stack) • [Getting Started](#getting-started) • [Architecture](#architecture) • [API Documentation](#api-documentation)

</div>

---

## 📋 Table of Contents

- [Overview](#overview)
- [Features](#features)
- [Tech Stack](#tech-stack)
- [Architecture](#architecture)
- [Getting Started](#getting-started)
  - [Prerequisites](#prerequisites)
  - [Backend Setup](#backend-setup)
  - [Frontend Setup](#frontend-setup)
- [Project Structure](#project-structure)
- [API Documentation](#api-documentation)
- [Database Schema](#database-schema)
- [AI Integration](#ai-integration)
- [Real-time Features](#real-time-features)
- [Payment Integration](#payment-integration)
- [Security](#security)
- [Contributing](#contributing)

---

## 🎯 Overview

**HireAI** is a comprehensive recruitment management system that leverages artificial intelligence to streamline the hiring process. The platform enables HR professionals to post jobs, manage applications, and automatically generate intelligent exams, while providing applicants with a seamless job search and application experience.

### Key Highlights

- 🤖 **AI-Powered Exam Generation** using Google Gemini 2.5 Flash
- 📊 **Real-time Notifications** via SignalR
- 💳 **Secure Payment Processing** with Stripe
- 🎨 **Modern UI/UX** built with Angular 20
- 🔒 **Enterprise-grade Security** with JWT authentication
- ☁️ **Cloud Storage** integration with AWS S3
- 📱 **Responsive Design** for all devices

---

## ✨ Features

### For HR Professionals

- **Job Management**
  - Create, update, and delete job postings
  - Set custom exam durations and difficulty levels
  - Track job post performance and analytics

- **Application Management**
  - Review applicant profiles and resumes
  - Filter and search applications
  - Track application status throughout the hiring pipeline

- **AI-Powered Exam Creation**
  - Automatically generate relevant technical questions based on job descriptions
  - Customize exam difficulty based on experience level
  - 10 multiple-choice questions with 4 options each

- **Dashboard & Analytics**
  - View hiring metrics and statistics
  - Monitor active job postings
  - Track applicant performance

- **Real-time Notifications**
  - Instant alerts for new applications
  - Application status updates
  - System notifications

### For Applicants

- **Job Discovery**
  - Browse available job opportunities
  - Advanced search and filtering
  - View detailed job descriptions

- **Application Process**
  - Easy one-click application submission
  - Upload resume and portfolio
  - Track application status in real-time

- **Mock Exams**
  - Practice with AI-generated mock exams
  - Prepare for actual job assessments
  - View exam results and feedback

- **Profile Management**
  - Create and update professional profile
  - Upload resume to AWS S3
  - Manage account settings

- **Real-time Notifications**
  - Application status updates
  - Exam invitations
  - Interview scheduling alerts

### Common Features

- **Secure Authentication**
  - JWT-based authentication
  - Role-based access control (HR/Applicant)
  - Password encryption and security

- **Payment Integration**
  - Premium job posting features
  - Subscription management
  - Secure payment processing via Stripe

- **Responsive Design**
  - Mobile-first approach
  - Cross-browser compatibility
  - Optimized performance

---

## 🛠️ Tech Stack

### Backend (.NET 9.0)

| Technology | Purpose |
|------------|---------|
| **ASP.NET Core 9.0** | Web API framework |
| **Entity Framework Core** | ORM for database operations |
| **SQL Server** | Relational database |
| **AutoMapper** | Object-to-object mapping |
| **JWT Bearer** | Authentication & authorization |
| **SignalR** | Real-time communication |
| **Stripe API** | Payment processing |
| **Google Gemini API** | AI exam generation |
| **AWS S3** | Cloud file storage |
| **Swagger/OpenAPI** | API documentation |

### Frontend (Angular 20.3)

| Technology | Purpose |
|------------|---------|
| **Angular 20.3** | Frontend framework |
| **TypeScript 5.9** | Type-safe JavaScript |
| **RxJS 7.8** | Reactive programming |
| **Bootstrap 5.3** | UI component library |
| **Chart.js 4.5** | Data visualization |
| **Bootstrap Icons** | Icon library |

### DevOps & Tools

- **Git** - Version control
- **GitHub** - Code repository
- **Swagger UI** - API testing
- **Prettier** - Code formatting

---

## 🏗️ Architecture

### System Architecture

```
┌─────────────────────────────────────────────────────────────┐
│                        Client Layer                          │
│  ┌──────────────┐  ┌──────────────┐  ┌──────────────┐      │
│  │   Angular    │  │  Bootstrap   │  │   Chart.js   │      │
│  │   Frontend   │  │      UI      │  │  Analytics   │      │
│  └──────────────┘  └──────────────┘  └──────────────┘      │
└─────────────────────────────────────────────────────────────┘
                            ▼
┌─────────────────────────────────────────────────────────────┐
│                      API Gateway Layer                       │
│  ┌──────────────┐  ┌──────────────┐  ┌──────────────┐      │
│  │     JWT      │  │    CORS      │  │   SignalR    │      │
│  │     Auth     │  │   Policies   │  │     Hubs     │      │
│  └──────────────┘  └──────────────┘  └──────────────┘      │
└─────────────────────────────────────────────────────────────┘
                            ▼
┌─────────────────────────────────────────────────────────────┐
│                     Business Logic Layer                     │
│  ┌──────────────┐  ┌──────────────┐  ┌──────────────┐      │
│  │   Services   │  │  AutoMapper  │  │  Validators  │      │
│  │   (BLL)      │  │   Profiles   │  │    Logic     │      │
│  └──────────────┘  └──────────────┘  └──────────────┘      │
└─────────────────────────────────────────────────────────────┘
                            ▼
┌─────────────────────────────────────────────────────────────┐
│                    Data Access Layer                         │
│  ┌──────────────┐  ┌──────────────┐  ┌──────────────┐      │
│  │ Repositories │  │  EF Core     │  │   Unit of    │      │
│  │   (DAL)      │  │   Context    │  │     Work     │      │
│  └──────────────┘  └──────────────┘  └──────────────┘      │
└─────────────────────────────────────────────────────────────┘
                            ▼
┌─────────────────────────────────────────────────────────────┐
│                    External Services                         │
│  ┌──────────────┐  ┌──────────────┐  ┌──────────────┐      │
│  │  SQL Server  │  │  Gemini AI   │  │   AWS S3     │      │
│  │   Database   │  │     API      │  │   Storage    │      │
│  └──────────────┘  └──────────────┘  └──────────────┘      │
│  ┌──────────────┐  ┌──────────────┐                        │
│  │   Stripe     │  │   SignalR    │                        │
│  │   Payments   │  │   Service    │                        │
│  └──────────────┘  └──────────────┘                        │
└─────────────────────────────────────────────────────────────┘
```

### Backend Project Structure

```
HireAI/
├── HireAI.API/                 # Web API Layer
│   ├── Controllers/            # API endpoints
│   ├── Extensions/             # Service extensions
│   ├── Program.cs              # Application entry point
│   └── appsettings.json        # Configuration
│
├── HireAI.Service/             # Business Logic Layer
│   ├── Services/               # Business logic implementation
│   ├── Interfaces/             # Service contracts
│   └── Mapping/                # AutoMapper profiles
│
├── HireAI.Infrastructure/      # Data Access Layer
│   ├── Context/                # EF Core DbContext
│   ├── Repositories/           # Data repositories
│   └── GenericBase/            # Generic repository pattern
│
├── HireAI.Data/                # Data Models & DTOs
│   ├── Models/                 # Entity models
│   └── Helpers/                # DTOs and helpers
│
└── HireAI.Core/                # Core utilities
    └── Shared/                 # Shared resources
```

### Frontend Project Structure

```
hire-ai-front/
├── src/
│   ├── app/
│   │   ├── core/               # Core functionality
│   │   │   ├── guards/         # Route guards
│   │   │   ├── interceptors/   # HTTP interceptors
│   │   │   ├── models/         # TypeScript models
│   │   │   └── services/       # API services
│   │   │
│   │   ├── features/           # Feature modules
│   │   │   ├── auth/           # Authentication
│   │   │   ├── hr/             # HR features
│   │   │   ├── applicant/      # Applicant features
│   │   │   └── landing/        # Landing page
│   │   │
│   │   ├── layouts/            # Layout components
│   │   │   ├── public-layout/  # Public pages layout
│   │   │   └── dashboard-layout/ # Dashboard layout
│   │   │
│   │   ├── shared/             # Shared components
│   │   └── app.routes.ts       # Application routes
│   │
│   ├── styles.css              # Global styles
│   └── index.html              # Entry HTML
│
└── package.json                # Dependencies
```

---

## 🚀 Getting Started

### Prerequisites

Before you begin, ensure you have the following installed:

- **.NET 9.0 SDK** - [Download](https://dotnet.microsoft.com/download/dotnet/9.0)
- **Node.js 20+** - [Download](https://nodejs.org/)
- **SQL Server 2019+** - [Download](https://www.microsoft.com/en-us/sql-server/sql-server-downloads)
- **Visual Studio 2022** or **VS Code** - [Download](https://visualstudio.microsoft.com/)
- **Git** - [Download](https://git-scm.com/)

### Backend Setup

1. **Clone the repository**
   ```bash
   git clone https://github.com/ahmedgndy/HireAI.git
   cd HireAI
   ```

2. **Configure the database connection**
   
   Update the connection string in `HireAI.API/appsettings.json`:
   ```json
   {
     "ConnectionStrings": {
       "HireAiDB": "Server=YOUR_SERVER;Database=HireAI;Trusted_Connection=True;TrustServerCertificate=True;"
     }
   }
   ```

3. **Configure API keys**
   
   Add your API keys to `appsettings.json`:
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
       "SecurityKey": "YOUR_SECRET_KEY_HERE_MINIMUM_32_CHARACTERS",
       "IssuerIP": "http://localhost:5290",
       "AudienceIP": "http://localhost:4200"
     }
   }
   ```

4. **Restore NuGet packages**
   ```bash
   dotnet restore
   ```

5. **Apply database migrations**
   ```bash
   dotnet ef database update --project HireAI.Infrastructure --startup-project HireAI.API
   ```

6. **Run the API**
   ```bash
   cd HireAI.API
   dotnet run
   ```

   The API will be available at `http://localhost:5290`
   
   Swagger UI: `http://localhost:5290/swagger`

### Frontend Setup

1. **Navigate to the frontend directory**
   ```bash
   cd HireAi_Angular-main
   ```

2. **Install dependencies**
   ```bash
   npm install
   ```

3. **Configure API endpoint**
   
   Update the API URL in your environment configuration if needed.

4. **Start the development server**
   ```bash
   npm start
   ```

   The application will be available at `http://localhost:4200`

### Quick Start with Sample Data

The application includes a database seeder that automatically creates sample data on first run:

- **HR Account**: `hr@hireai.com` / `Password123!`
- **Applicant Account**: `applicant@hireai.com` / `Password123!`
- Sample job postings
- Sample mock exams

---

## 📁 Project Structure

### Backend Layers

#### 1. **HireAI.API** - Presentation Layer
- RESTful API controllers
- Middleware configuration
- Dependency injection setup
- Authentication & authorization

#### 2. **HireAI.Service** - Business Logic Layer
- Business logic implementation
- Service interfaces and implementations
- AutoMapper configuration
- Validation logic

#### 3. **HireAI.Infrastructure** - Data Access Layer
- Entity Framework Core context
- Repository pattern implementation
- Database migrations
- Data seeding

#### 4. **HireAI.Data** - Data Models
- Entity models
- DTOs (Data Transfer Objects)
- Enumerations
- Helper classes

#### 5. **HireAI.Core** - Core Utilities
- Shared resources
- Common utilities
- Constants and configurations

---

## 📚 API Documentation

### Authentication Endpoints

#### Register HR
```http
POST /api/Account/register-hr
Content-Type: application/json

{
  "email": "hr@example.com",
  "password": "SecurePassword123!",
  "companyName": "Tech Corp",
  "firstName": "John",
  "lastName": "Doe"
}
```

#### Register Applicant
```http
POST /api/Account/register-applicant
Content-Type: application/json

{
  "email": "applicant@example.com",
  "password": "SecurePassword123!",
  "firstName": "Jane",
  "lastName": "Smith"
}
```

#### Login
```http
POST /api/Account/login
Content-Type: application/json

{
  "email": "user@example.com",
  "password": "SecurePassword123!"
}

Response:
{
  "token": "eyJhbGciOiJIUzI1NiIsInR5cCI6IkpXVCJ9...",
  "expiration": "2024-12-11T14:00:00Z",
  "role": "HR"
}
```

### Job Management Endpoints

#### Create Job Post
```http
POST /api/JobPost
Authorization: Bearer {token}
Content-Type: application/json

{
  "title": "Senior .NET Developer",
  "description": "We are looking for an experienced .NET developer...",
  "companyName": "Tech Corp",
  "experienceLevel": "Senior",
  "examDurationMinutes": 45,
  "location": "Remote",
  "salary": "$100,000 - $150,000"
}
```

#### Get All Jobs
```http
GET /api/AvailableJobs
Authorization: Bearer {token}
```

#### Get Job Details
```http
GET /api/JobPost/{jobId}
Authorization: Bearer {token}
```

#### Update Job
```http
PUT /api/JobPost/{jobId}
Authorization: Bearer {token}
Content-Type: application/json
```

#### Delete Job
```http
DELETE /api/JobPost/{jobId}
Authorization: Bearer {token}
```

### Application Endpoints

#### Submit Application
```http
POST /api/Application
Authorization: Bearer {token}
Content-Type: application/json

{
  "jobId": 1,
  "applicantId": 5,
  "coverLetter": "I am excited to apply..."
}
```

#### Get Applicant's Applications
```http
GET /api/Application/applicant/{applicantId}
Authorization: Bearer {token}
```

#### Get Job Applications (HR)
```http
GET /api/Application/job/{jobId}
Authorization: Bearer {token}
```

#### Update Application Status
```http
PUT /api/Application/{applicationId}/status
Authorization: Bearer {token}
Content-Type: application/json

{
  "status": "Accepted"
}
```

### AI Exam Generation Endpoints

#### Create Job Exam with AI
```http
POST /api/Exam/job-exam/{applicationId}
Authorization: Bearer {token}

Response:
{
  "message": "Job exam created successfully with AI-generated questions",
  "questionCount": 10,
  "questions": [
    {
      "id": 101,
      "questionText": "What is dependency injection?",
      "questionNumber": 1,
      "examId": 50
    }
  ]
}
```

#### Create Mock Exam with AI
```http
POST /api/Exam/mock-exam/{examId}
Authorization: Bearer {token}

Response:
{
  "message": "Mock exam questions generated successfully",
  "questionCount": 10,
  "questions": [...]
}
```

#### Get Exam Details
```http
GET /api/Exam/{examId}
Authorization: Bearer {token}
```

### Dashboard Endpoints

#### HR Dashboard
```http
GET /api/HRDashboard
Authorization: Bearer {token}

Response:
{
  "totalJobs": 15,
  "activeJobs": 10,
  "totalApplications": 150,
  "pendingApplications": 45
}
```

#### Applicant Dashboard
```http
GET /api/ApplicantDashboard
Authorization: Bearer {token}

Response:
{
  "totalApplications": 5,
  "pendingApplications": 2,
  "acceptedApplications": 2,
  "rejectedApplications": 1
}
```

### File Upload Endpoints

#### Upload Resume
```http
POST /api/Applicant/upload-resume
Authorization: Bearer {token}
Content-Type: multipart/form-data

{
  "file": [binary data]
}

Response:
{
  "fileUrl": "https://bucket.s3.amazonaws.com/resumes/resume.pdf",
  "fileName": "resume.pdf"
}
```

---

## 🗄️ Database Schema

### Core Entities

#### ApplicationUser
- Identity-based user management
- Roles: HR, Applicant
- Profile information
- Authentication data

#### JobPost
- Job title and description
- Company information
- Experience level requirements
- Exam duration settings
- Created by HR user

#### Application
- Links applicant to job post
- Application status tracking
- Submission timestamp
- Cover letter
- Associated exam

#### Exam
- Exam type (MockExam, HrExam)
- Difficulty level
- Duration in minutes
- AI-generated flag
- Question collection

#### Question
- Question text
- Multiple choice options (4 choices)
- Correct answer index (0-3)
- Question number
- Associated exam

#### ApplicantResponse
- Applicant's answer
- Question reference
- Exam submission

### Relationships

```
ApplicationUser (HR) 1──────────* JobPost
ApplicationUser (Applicant) 1──* Application
JobPost 1──────────────────────* Application
Application 1──────────────────1 Exam
Exam 1─────────────────────────* Question
Question 1─────────────────────* ApplicantResponse
```

---

## 🤖 AI Integration

### Google Gemini 2.5 Flash API

HireAI uses Google's Gemini 2.5 Flash model for intelligent exam generation.

#### Features

- **Automatic Question Generation**: Creates relevant technical questions based on job descriptions
- **Difficulty Adjustment**: Adapts question complexity to experience level
- **Multiple Choice Format**: Generates 4 options per question with one correct answer
- **Rate Limiting**: Respects API limits (15 requests/min for free tier)
- **Token Optimization**: Efficient prompts to minimize costs

#### Implementation Details

**Service**: `GeminiService.cs`

**Key Methods**:
- `GenerateJobExamQuestionsAsync(string jobDescription)`
- `GenerateMockExamQuestionsAsync(string examDescription)`

**Configuration**:
```csharp
generationConfig = new
{
    temperature = 0.7,      // Balanced creativity
    maxOutputTokens = 4096, // Sufficient for 10 questions
    topP = 0.9,
    topK = 40
}
```

**Rate Limiting Strategy**:
```csharp
// Enforces ~4 seconds between requests
private const int MinMillisecondsBetweenRequests = 4000;
```

#### Sample AI Prompt

```
Generate 10 multiple-choice questions (MCQ) for a job exam based on the following description.

Description: {jobDescription}

Requirements:
1. Generate exactly 10 questions
2. Each question must have exactly 4 choices
3. Each question must have one correct answer
4. Questions should be relevant to the job description
5. Difficulty should match the experience level
6. Index the correct answer (0-3)

Return ONLY a JSON response in this exact format:
{
  "questions": [
    {
      "questionText": "<question>",
      "choices": ["<choice 0>", "<choice 1>", "<choice 2>", "<choice 3>"],
      "correctAnswerIndex": <0-3>
    }
  ]
}
```

#### Error Handling

- Safety filter violations
- Token limit exceeded
- JSON parsing errors
- Network failures
- Rate limit enforcement

For detailed implementation, see [AI_EXAM_GENERATION_IMPLEMENTATION.md](./AI_EXAM_GENERATION_IMPLEMENTATION.md)

---

## 🔔 Real-time Features

### SignalR Integration

HireAI implements real-time notifications using SignalR for instant updates.

#### Features

- **Application Status Updates**: Notify applicants when their application status changes
- **New Application Alerts**: Alert HR when new applications are submitted
- **Exam Invitations**: Real-time exam invitation notifications
- **System Notifications**: General system alerts and announcements

#### Hub Implementation

**NotificationHub**:
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

#### Client Connection (Angular)

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
      .catch(err => console.error('SignalR Error:', err));
  }
  
  addNotificationListener() {
    this.hubConnection.on('ReceiveNotification', (message) => {
      this.showNotification(message);
    });
  }
}
```

#### Notification Types

| Event | Trigger | Recipient |
|-------|---------|-----------|
| `NewApplication` | Application submitted | HR User |
| `ApplicationStatusChanged` | Status updated by HR | Applicant |
| `ExamInvitation` | Exam assigned | Applicant |
| `SystemAlert` | Admin action | All Users |

---

## 💳 Payment Integration

### Stripe Payment Processing

HireAI integrates Stripe for secure payment processing.

#### Features

- **Premium Job Postings**: HR can pay for featured job listings
- **Subscription Plans**: Monthly/yearly subscription options
- **Secure Checkout**: PCI-compliant payment processing
- **Payment History**: Track all transactions
- **Refund Management**: Handle refunds and disputes

#### Implementation

**Payment Service**:
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

#### Payment Endpoints

```http
POST /api/Payment/create-payment-intent
Authorization: Bearer {token}
Content-Type: application/json

{
  "amount": 99.99,
  "currency": "usd",
  "description": "Premium Job Posting"
}

Response:
{
  "clientSecret": "pi_xxx_secret_xxx",
  "paymentIntentId": "pi_xxx"
}
```

```http
POST /api/Payment/create-subscription
Authorization: Bearer {token}
Content-Type: application/json

{
  "priceId": "price_xxx",
  "customerId": "cus_xxx"
}
```

#### Pricing Plans

| Plan | Price | Features |
|------|-------|----------|
| **Free** | $0/month | 5 job posts, Basic analytics |
| **Professional** | $49/month | 25 job posts, Advanced analytics, Priority support |
| **Enterprise** | $199/month | Unlimited posts, Custom branding, Dedicated support |

#### Webhook Handling

```csharp
[HttpPost("webhook")]
public async Task<IActionResult> StripeWebhook()
{
    var json = await new StreamReader(HttpContext.Request.Body).ReadToEndAsync();
    var stripeEvent = EventUtility.ConstructEvent(json, Request.Headers["Stripe-Signature"], _webhookSecret);
    
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
    }
    
    return Ok();
}
```

---

## 🔒 Security

### Authentication & Authorization

- **JWT Bearer Tokens**: Stateless authentication
- **Role-based Access Control**: HR and Applicant roles
- **Password Hashing**: ASP.NET Core Identity with bcrypt
- **Token Expiration**: Configurable token lifetime
- **Refresh Tokens**: Secure token renewal

### API Security

- **HTTPS Enforcement**: TLS 1.2+ required in production
- **CORS Policies**: Configured allowed origins
- **Rate Limiting**: Prevent API abuse
- **Input Validation**: Data annotation and FluentValidation
- **SQL Injection Prevention**: Parameterized queries via EF Core

### Data Protection

- **Encryption at Rest**: Database encryption
- **Encryption in Transit**: HTTPS/TLS
- **Sensitive Data Masking**: PII protection
- **AWS S3 Security**: Private buckets with signed URLs
- **Stripe PCI Compliance**: No card data stored

### Best Practices

```csharp
// JWT Configuration
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

---

## 🧪 Testing

### Backend Testing

```bash
# Run all tests
dotnet test

# Run with coverage
dotnet test /p:CollectCoverage=true
```

### Frontend Testing

```bash
# Run unit tests
npm test

# Run e2e tests
npm run e2e
```

---

## 📈 Performance Optimization

- **Database Indexing**: Optimized queries
- **Caching**: Redis integration ready
- **Lazy Loading**: Angular lazy-loaded modules
- **CDN**: Static assets served via CDN
- **Compression**: Gzip/Brotli compression
- **Pagination**: Efficient data loading

---

## 🤝 Contributing

We welcome contributions! Please follow these steps:

1. Fork the repository
2. Create a feature branch (`git checkout -b feature/AmazingFeature`)
3. Commit your changes (`git commit -m 'Add some AmazingFeature'`)
4. Push to the branch (`git push origin feature/AmazingFeature`)
5. Open a Pull Request

### Coding Standards

- Follow C# coding conventions
- Use Angular style guide
- Write meaningful commit messages
- Add unit tests for new features
- Update documentation

---

## 👥 Team

- **Backend Development**: .NET Core API, Database Design, AI Integration
- **Frontend Development**: Angular SPA, UI/UX Design
- **DevOps**: CI/CD, Cloud Infrastructure

---

## 📞 Support

For support, email support@hireai.com or open an issue in the repository.

---

## 🗺️ Roadmap

- [ ] Video interview integration
- [ ] Advanced analytics dashboard
- [ ] Mobile application (iOS/Android)
- [ ] Multi-language support
- [ ] AI-powered resume parsing
- [ ] Automated interview scheduling
- [ ] Integration with LinkedIn
- [ ] Advanced reporting tools

---

## 📝 Additional Documentation

- [Quick Start Guide](./QUICK_START_GUIDE.md) - Get started quickly
- [AI Implementation Details](./AI_EXAM_GENERATION_IMPLEMENTATION.md) - Deep dive into AI features
- [API Reference](http://localhost:5290/swagger) - Complete API documentation

---

<div align="center">

**Built with ❤️ by the HireAI Team**

[⬆ Back to Top](#hireai---ai-powered-recruitment-platform)

</div>
