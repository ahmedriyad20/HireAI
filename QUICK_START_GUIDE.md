# Quick Start Guide - AI Exam Generation Endpoints

## Prerequisites
1. Ensure the application is running on `http://localhost:5290`
2. Have a valid JWT token for authentication
3. Have test data in the database:
   - An Application record with a valid JobId
   - A JobPost with a Description
   - An Exam with an ExamDescription (for mock exam testing)

## Endpoint 1: Create Job Exam with AI Questions

### Request
```http
POST http://localhost:5290/api/Exam/job-exam/{applicationId}
Authorization: Bearer YOUR_JWT_TOKEN
Content-Type: application/json
```

### Example Using cURL
```bash
curl -X POST "http://localhost:5290/api/Exam/job-exam/1" \
  -H "Authorization: Bearer eyJhbGciOiJIUzI1NiIsInR5cCI6IkpXVCJ9..." \
  -H "Content-Type: application/json"
```

### Example Using Swagger
1. Navigate to `http://localhost:5290/swagger`
2. Find `POST /api/Exam/job-exam/{applicationId}`
3. Click "Try it out"
4. Enter an `applicationId` (e.g., `1`)
5. Click "Execute"

### Success Response (200 OK)
```json
{
  "message": "Job exam created successfully with AI-generated questions",
  "questionCount": 10,
  "questions": [
    {
      "id": 101,
      "questionText": "What is the main purpose of dependency injection in .NET?",
      "questionNumber": 1,
      "examId": 50,
      "answer": null,
      "applicantResponseId": null,
      "answers": []
    },
    {
      "id": 102,
      "questionText": "Which design pattern is commonly used for creating objects?",
      "questionNumber": 2,
      "examId": 50,
      "answer": null,
      "applicantResponseId": null,
      "answers": []
    }
    // ... 8 more questions
  ]
}
```

### Error Responses

#### 400 Bad Request
```json
{
  "error": "Application not found"
}
```

```json
{
  "error": "Job post does not have a description"
}
```

#### 403 Forbidden
If the user doesn't have permission to access the application

---

## Endpoint 2: Create Mock Exam Questions with AI

### Request
```http
POST http://localhost:5290/api/Exam/mock-exam/{examId}
Authorization: Bearer YOUR_JWT_TOKEN
Content-Type: application/json
```

### Example Using cURL
```bash
curl -X POST "http://localhost:5290/api/Exam/mock-exam/5" \
  -H "Authorization: Bearer eyJhbGciOiJIUzI1NiIsInR5cCI6IkpXVCJ9..." \
  -H "Content-Type: application/json"
```

### Example Using Postman
1. Create a new POST request
2. URL: `http://localhost:5290/api/Exam/mock-exam/5`
3. Headers:
   - `Authorization: Bearer YOUR_TOKEN`
   - `Content-Type: application/json`
4. Click "Send"

### Success Response (200 OK)
```json
{
  "message": "Mock exam questions generated successfully",
  "questionCount": 10,
  "questions": [
    {
      "id": 201,
      "questionText": "Which data structure uses LIFO (Last In First Out) principle?",
      "questionNumber": 1,
      "examId": 5,
      "answer": null,
      "applicantResponseId": null,
      "answers": []
    },
    {
      "id": 202,
      "questionText": "What is the time complexity of binary search?",
      "questionNumber": 2,
      "examId": 5,
      "answer": null,
      "applicantResponseId": null,
      "answers": []
    }
    // ... 8 more questions
  ]
}
```

### Error Responses

#### 400 Bad Request
```json
{
  "error": "Exam not found"
}
```

```json
{
  "error": "Exam does not have a description"
}
```

---

## Viewing Generated Questions in Database

After creating an exam, you can query the questions:

### Get Exam with Questions
```http
GET http://localhost:5290/api/Exam/{examId}
Authorization: Bearer YOUR_JWT_TOKEN
```

This will return the exam with all questions including:
- Question text
- 4 multiple choice options (stored in `Choices` array)
- Correct answer index (0-3)

---

## Important Notes

### Rate Limiting
- The system automatically enforces rate limits (15 requests/min for free tier)
- If you make multiple requests quickly, there will be automatic delays (~4 seconds)
- This is normal behavior to comply with Gemini API limits

### Token Usage
- Each request uses approximately 500-1000 tokens
- Free tier limit: 15 requests per minute
- The implementation optimizes for minimal token usage

### Question Format
Questions are stored in the `Questions` table with:
```sql
SELECT 
    Id,
    QuestionText,
    QuestionNumber,
    CorrectAnswerIndex,  -- Value between 0-3
    ExamId
FROM Questions
WHERE ExamId = 50
ORDER BY QuestionNumber;
```

Choices are stored in a string array (4 items) but not exposed in the basic response DTO for security reasons (to prevent showing answers before exam is taken).

---

## Testing Workflow

### 1. Create Test Data (if needed)

#### Create a Job Post with Description
```http
POST /api/JobPost
{
  "title": "Senior .NET Developer",
  "description": "We are looking for an experienced .NET developer with expertise in C#, ASP.NET Core, Entity Framework, and SQL Server. The candidate should have strong knowledge of design patterns, SOLID principles, and microservices architecture.",
  "companyName": "Tech Corp",
  "experienceLevel": "Senior",
  "examDurationMinutes": 45
}
```

#### Create a Mock Exam
```http
POST /api/Exam
{
  "examName": "Data Structures & Algorithms",
  "examDescription": "Test your knowledge of fundamental data structures including arrays, linked lists, stacks, queues, trees, graphs, and common algorithms like sorting, searching, and graph traversal.",
  "examLevel": "Intermediate",
  "numberOfQuestions": 10,
  "durationInMinutes": 30,
  "examType": "MockExam"
}
```

### 2. Apply to Job (creates Application)
```http
POST /api/Application
{
  "jobId": 1,
  "applicantId": 5
}
```

### 3. Generate Exam Questions
```http
POST /api/Exam/job-exam/1
```

### 4. Verify Questions Were Created
```http
GET /api/Exam/{examId}
```

---

## Troubleshooting

### Error: "Application not found"
- Verify the applicationId exists in the database
- Check that you have permission to access this application

### Error: "Job post does not have a description"
- Update the job post to include a description
- Description should be detailed enough for AI to generate relevant questions

### Error: "Gemini API error"
- Check that the Gemini API key is valid in appsettings.json
- Verify internet connectivity
- Check if you've exceeded the free tier rate limits (wait a minute and retry)

### Slow Response Times
- AI generation takes 3-8 seconds typically
- Rate limiting adds 4-second delays between requests
- This is normal behavior

### Questions Are Too Easy/Hard
- Modify the job/exam description to include difficulty indicators
- For job exams, the difficulty is auto-adjusted based on experience level
- Add specific topics you want to test in the description

---

## Sample Test Scenarios

### Scenario 1: Entry-Level Developer Exam
```http
POST /api/Exam/job-exam/1
# Application linked to job with experience level "EntryLevel"
# Generates beginner-level questions
```

### Scenario 2: Senior Developer Exam
```http
POST /api/Exam/job-exam/2
# Application linked to job with experience level "Senior"
# Generates advanced-level questions
```

### Scenario 3: Algorithm Mock Exam
```http
POST /api/Exam/mock-exam/5
# Exam with description about algorithms
# Generates algorithm-focused questions
```

---

## Next Steps

After generating questions, you can:
1. View the exam in the applicant/HR dashboard
2. Take the exam (as applicant)
3. Evaluate exam results (as HR)
4. Regenerate questions if needed (create new exam)

---

## API Rate Limits Summary

| Tier | Requests/Min | Requests/Day | Tokens/Min |
|------|--------------|--------------|------------|
| Free | 15 | 1,500 | 32,000 |

**Current Implementation:** Automatically throttles to ~15 requests/min with 4-second delays between calls.

