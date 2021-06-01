using System;
using Quap.Models;
using Quap.Services.UserManagement;

namespace Quap.Seed
{
    public class SeedDataHelper
    {
        public static void Seed(IUserManagementService users, QuapDbContext db)
        {
            User user1 = users.register(new RegisterRequest()
            {
                username = "test_user1",
                password = "test_user1"
            });
            User user2 = users.register(new RegisterRequest()
            {
                username = "test_user2",
                password = "test_user2"
            });
            User user3 = users.register(new RegisterRequest()
            {
                username = "test_user3",
                password = "test_user3"
            });

            Question question1 = db.Questions.Add(new Question()
            {
                title = "Rxjs Subject not emitting data immediately",
                body =
@"I have a service with a RXJS subject and i am assigning data to subject inside constructor doing an api call. This subject I am subscribing in a component template. Though the data is provided to the subject, it is not emitting immediately the first time.
                
```
interface Employee {
employee_age: number;
employee_name: string;
employee_salary: number;
id: string;
profile_image: string;
}

@Injectable({
  providedIn: 'root',
})
export class EmployeeService {
 employeesSub = new Subject<Employee[]>();

 employees: Employee[];

 constructor(private http: HttpClient) {

this.api().subscribe((res) => {
  this.employees = res.data;
  this.employeesSub.next(this.employees);
});

}

 getEmployees(){
     this.employeesSub.next(this.employees);
 }

 addEmployee(name,age,salary) {
    this.employees.unshift({id:(this.employees.length + 
 1).toString(),employee_age:age,employee_name:name,employee_salary:salary,profile_image:""});
   this.employeesSub.next(this.employees);
 }
 
 api() {
    return this.http
     .get<any>(environment.employeeUrl)
     .pipe(map((data) => data.items));
   }
  }

 Code in template

  <h2>List</h2>
  <div style=""display: flex;""></div>
  <table>
     <tr *ngFor=""let item of employeeService.employeesSub|async"">
       <td>      {{ item.employee_name}}    </td>
       <td>      {{ item.employee_age}}    </td>
       <td>      {{ item.employee_salary}}    </td>
     </tr>
  </table>
  ```

  I am reassigning data by calling the getEmployees() function after 200ms and it is working. Any idea why this is happening.
                ",
                createdById = user1.id,
                created = new DateTime(2021, 1, 22),
                lastModified = new DateTime(2021, 1, 22),
            }).Entity;

            db.QuestionComments.AddRange(
                new QuestionComment()
                {
                    created = new DateTime(2021, 2, 22),
                    lastModified = new DateTime(2021, 2, 22),
                    commenterId = user1.id,
                    comment = "This is a comment!",
                    questionId = question1.id
                },
                new QuestionComment()
                {
                    created = new DateTime(2021, 2, 22),
                    lastModified = new DateTime(2021, 2, 22),
                    commenterId = user2.id,
                    comment = "Have you tried turning it off and on again? I know it's weird, but that helps a lot!",
                    questionId = question1.id
                }
            );

            Question question2 = db.Questions.Add(new Question()
            {
                title = "Test Question 2",
                body = "This is a question",
                createdById = user2.id,
                created = new DateTime(2021, 4, 22),
                lastModified = new DateTime(2021, 4, 22),
            }).Entity;

            Question question3 = db.Questions.Add(new Question()
            {
                title = "Test Question 3",
                body = "This is a question",
                createdById = user3.id,
                created = new DateTime(2021, 5, 22),
                lastModified = new DateTime(2021, 5, 22),
            }).Entity;

            Answer answer1 = db.Answers.Add(new Answer()
            {
                body =
@"
You need to switch to a [BehaviorSubject][1].

The service gets initialized before the component does, so it's emitting the value before the component gets to subscribe. Since a Subject doesn't hold a value, the component doesn't get anything. By swapping to a Behavior subject, you can subscribe to it and immediately get the latest value.

The accepted answer here [describes the difference been a `Subject` and a `BehaviorSubject`][2] well.


```
// Init the subject with a starting value, to be updated in the constructor later
employeesSub = new BehaviorSubject<Employee[]>([]);

constructor(private http: HttpClient) {
    this.api().subscribe((res) => {
        this.employees = res.data;
        this.employeesSub.next(this.employees);
    });
}
```


  [1]: https://www.learnrxjs.io/learn-rxjs/subjects/behaviorsubject
  [2]: https://stackoverflow.com/questions/43348463/what-is-the-difference-between-subject-and-behaviorsubject
",
                createdById = user2.id,
                questionId = question1.id,
                created = new DateTime(2021, 5, 23),
                lastModified = new DateTime(2021, 5, 23),
                accepted = false
            }).Entity;

            db.Answers.Add(new Answer()
            {
                body = "This is an answer",
                createdById = user1.id,
                questionId = question1.id,
                created = new DateTime(2021, 5, 23),
                lastModified = new DateTime(2021, 5, 23),
                accepted = false
            });

            db.Answers.Add(new Answer()
            {
                body = "This is an answer",
                createdById = user1.id,
                questionId = question3.id,
                created = new DateTime(2021, 5, 23),
                lastModified = new DateTime(2021, 5, 23),
                accepted = true
            });

            db.AnswerComments.AddRange(
                new AnswerComment()
                {
                    created = new DateTime(2021, 5, 23),
                    lastModified = new DateTime(2021, 5, 23),
                    commenterId = user1.id,
                    comment = "Wow this is a really great answer.",
                    answerId = answer1.id
                },
                new AnswerComment()
                {
                    created = new DateTime(2021, 5, 23),
                    lastModified = new DateTime(2021, 5, 23),
                    commenterId = user2.id,
                    comment = "This guy knows how to write the codes really good. Much great answer contents here.",
                    answerId = answer1.id
                }
            );

            Tag tag1 = db.Tags.Add(new Tag()
            {
                name = "Tag 1",
                description = "Tag Description"
            }).Entity;

            db.QuestionTags.AddRange(
                new QuestionTag()
                {
                    questionId = question1.id,
                    tagId = tag1.id
                },
                new QuestionTag()
                {
                    questionId = question2.id,
                    tagId = tag1.id
                },
                new QuestionTag()
                {
                    questionId = question3.id,
                    tagId = tag1.id
                }
            );

            db.QuestionVotes.AddRange(
                new QuestionVote()
                {
                    questionId = question1.id,
                    voterId = user1.id,
                    voteType = VoteTypes.UPVOTE
                },
                new QuestionVote()
                {
                    questionId = question1.id,
                    voterId = user2.id,
                    voteType = VoteTypes.UPVOTE
                },
                new QuestionVote()
                {
                    questionId = question2.id,
                    voterId = user1.id,
                    voteType = VoteTypes.DOWNVOTE
                }
            );

            db.AnswerVotes.AddRange(
                new AnswerVote()
                {
                    answerId = answer1.id,
                    voterId = user1.id,
                    voteType = VoteTypes.UPVOTE
                },
                new AnswerVote()
                {
                    answerId = answer1.id,
                    voterId = user2.id,
                    voteType = VoteTypes.DOWNVOTE
                }
            );

            for (int i = 0; i < 100; i++)
            {
                db.Questions.Add(new Question()
                {
                    title = "Test Question " + i,
                    body = "This is a question",
                    createdById = user2.id,
                    created = new DateTime(2021, 4, 22),
                    lastModified = new DateTime(2021, 4, 22),
                });
            }

            db.SaveChanges();
        }
    }
}