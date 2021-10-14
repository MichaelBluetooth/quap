using System;
using Quap.Models;
using Quap.Services.UserManagement;

namespace Quap.Seed
{
    public class SeedDataHelper
    {
        public static void Seed(IUserManagementService users, QuapDbContext db)
        {
            User admin1 = users.register(new RegisterRequest()
            {
                username = "admin_user1",
                password = "admin_user1",
                role = User.Roles.ADMIN
            });
            User mod1 = users.register(new RegisterRequest()
            {
                username = "mod_user1",
                password = "mod_user1",
                role = User.Roles.MODERATOR
            });
            User user1 = users.register(new RegisterRequest()
            {
                username = "test_user1",
                password = "test_user1",
                role = User.Roles.USER
            });
            User user2 = users.register(new RegisterRequest()
            {
                username = "test_user2",
                password = "test_user2",
                role = User.Roles.USER
            });
            User user3 = users.register(new RegisterRequest()
            {
                username = "test_user3",
                password = "test_user3",
                role = User.Roles.USER
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

            Question question4 = db.Questions.Add(new Question()
            {
                title = "Using async pipe with ngIf",
                body = @"For the `status` property declared and populated like below:

```
public status:Promise<String>;

constructor() {
  this.status = this.getStatus();
}

public getStatus():Promise<String>{
  return new Promise((resolve,reject)=>{
      setTimeout(()=>{
        resolve('stable');
      },2500);
  });
}
```

could somebody explain how the below `async` pipe works?
```
<span *ngIf=""status|async"">
    {{ status|async }}
</span>
```",
                createdById = user3.id,
                created = new DateTime(2021, 10, 13),
                lastModified = new DateTime(2021, 10, 13)
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

            db.Answers.Add(new Answer()
            {
                body = @"I tend to combine `*ngIf` and `async` like so:

My component will have an Observable (or in your case a Promise), with a variable name that ends with `$`. This naming pattern comes from [the observable naming guide here][1] 

```
@Component({
  selector: 'my-app',
  templateUrl: './app.component.html',
  styleUrls: ['./app.component.css'],
})
export class AppComponent {
  //Create a subject with an initial value
  //Keep the subject private so only this component may emit value
  private _mySubject = new BehaviorSubject<any>('initial value!');

  //Expose the observable as a public variable
  //This allows the template to listen for values
  get myObservable$() {
    return this._mySubject.asObservable();
  }

  //helpers for demo!

  emitNull() {
    this._mySubject.next(null);
  }

  emitUndefined() {
    this._mySubject.next(undefined);
  }

  emitNumber(number) {
    this._mySubject.next(number);
  }

  emitText(text) {
    console.log(text);
    this._mySubject.next(text);
  }
}
```

Then my template:
```
<div>
  <button (click)=""emitNull()"">Emit Null</button>
</div>
<div>
  <button (click)=""emitUndefined()"">Emit Undefined</button>
</div>
<div>
  <button (click)=""emitNumber(num.value)"">Emit Number</button>
  <input type=""number"" #num value=""42"" />
</div>
<div>
  <button (click)=""emitText(txt.value)"">Emit String</button>
  <input type=""text"" #txt value=""foo"" />
</div>

<br />

<h1>Value:</h1>
<ng-container *ngIf=""myObservable$ | async as value; else other"">
  <div>{{ value }}</div>
</ng-container>

<ng-template #other>
  <div>The value was null, undefined or empty string</div>
</ng-template>
```

The template essentially reads as:

```

if(somevalue) 
	render a div displaying the value
else 
	render a div with text ""The value was null...""
```
	
The key thing is that `async` is a `pipe`. A `pipe` always transforms some input. In this case, we're passing in an observable (or a promise) and getting some output.

So putting it all together, the template is:
1. Subscribing to the observable (or `then'ing` in the case of a promise),
2. Outputting a something whenever a new value is emitted, capturing it in a variable named `value`
3. Performing the `if` check on `value`
4. Conditionally rendering the stuff inside `ng-container` or using the template marked with `#other`

[Here's a stackblitz demonstrating the above!][2]


As an aside, I recognize my example is using Observable instead of Promises. As I understand it, they essentially work the same. However, I strongly recommend using Observables over Promises in any Angular application. Observables are far more flexible and I think you'll run into far less confusing behavior.

  [1]: https://angular.io/guide/rx-library#naming-conventions-for-observables
  [2]: https://stackblitz.com/edit/angular-gjmfek?file=src/app/app.component.html",
                createdById = user1.id,
                questionId = question4.id,
                created = new DateTime(2021, 10, 13),
                lastModified = new DateTime(2021, 10, 13),
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
                description = "Tag Description",
                created = new DateTime(2020, 11, 20),
                lastModified = new DateTime(2020, 11, 20)
            }).Entity;

            Tag angularTag = db.Tags.Add(new Tag()
            {
                name = "Angular",
                description = "Questions about Angular, the SPA web framework from Google.",
                created = new DateTime(2020, 11, 20),
                lastModified = new DateTime(2020, 11, 20)
            }).Entity;

            Tag observableTag = db.Tags.Add(new Tag()
            {
                name = "Observable",
                description = "An observable is typically a programming construct that can be \"watched\" by other parts of the code, called the \"observers\". Different frameworks and programming languages have different implementations for observables, so this tag should typically be used in conjunction with others.",
                created = new DateTime(2020, 11, 20),
                lastModified = new DateTime(2020, 11, 20)
            }).Entity;

            Tag asyncTag = db.Tags.Add(new Tag()
            {
                name = "async",
                description = "Questions pertaining to asynchronous programming.",
                created = new DateTime(2020, 11, 20),
                lastModified = new DateTime(2020, 11, 20)
            }).Entity;

            db.Tags.Add(new Tag()
            {
                name = ".net",
                description = "Questions pertaining to .NET"
            });

            db.Tags.Add(new Tag()
            {
                name = "C#",
                description = "Questions pertaining to the C# programming language",
                created = new DateTime(2020, 11, 20),
                lastModified = new DateTime(2020, 11, 20)
            });

            db.Tags.Add(new Tag()
            {
                name = "Java",
                description = "Questions pertaining to the Java programming language",
                created = new DateTime(2020, 11, 20),
                lastModified = new DateTime(2020, 11, 20)
            });

            db.Tags.Add(new Tag()
            {
                name = "Python",
                description = "Questions pertaining to the Python programming language",
                created = new DateTime(2020, 11, 20),
                lastModified = new DateTime(2020, 11, 20)
            });

            db.Tags.Add(new Tag()
            {
                name = "Typescript",
                description = "Questions pertaining to Typescript",
                created = new DateTime(2020, 11, 20),
                lastModified = new DateTime(2020, 11, 20)
            });

            db.Tags.Add(new Tag()
            {
                name = "Javascript",
                description = "Questions pertaining to the Javascript programming language",
                created = new DateTime(2020, 11, 20),
                lastModified = new DateTime(2020, 11, 20)
            });

            db.Tags.Add(new Tag()
            {
                name = "SQL",
                description = "Questions pertaining to SQL (structured query language). This tag should typically be used in conjuction with a specific database technology",
                created = new DateTime(2020, 11, 20),
                lastModified = new DateTime(2020, 11, 20)
            });

            db.Tags.Add(new Tag()
            {
                name = "Oracle",
                description = "Questions pertaining to the Oracle database",
                created = new DateTime(2020, 11, 20),
                lastModified = new DateTime(2020, 11, 20)
            });

            db.Tags.Add(new Tag()
            {
                name = "MariaDB",
                description = "Questions pertaining to MariaDb",
                created = new DateTime(2020, 11, 20),
                lastModified = new DateTime(2020, 11, 20)
            });

            db.Tags.Add(new Tag()
            {
                name = "MongoDB",
                description = "Questions pertaining to MongoDB, a non-relational database",
                created = new DateTime(2020, 11, 20),
                lastModified = new DateTime(2020, 11, 20)
            });

            db.Tags.Add(new Tag()
            {
                name = "PostgreSQL",
                description = "PostgreSQL, also known as Postgres, is a free and open-source relational database management system emphasizing extensibility and SQL compliance. It was originally named POSTGRES, referring to its origins as a successor to the Ingres database developed at the University of California, Berkeley",
                created = new DateTime(2020, 11, 20),
                lastModified = new DateTime(2020, 11, 20)
            });

            db.Tags.Add(new Tag()
            {
                name = "Azure",
                description = "Microsoft Azure, commonly referred to as Azure, is a cloud computing service created by Microsoft for building, testing, deploying, and managing applications and services through Microsoft-managed data centers",
                created = new DateTime(2020, 11, 20),
                lastModified = new DateTime(2020, 11, 20)
            });

            db.Tags.Add(new Tag()
            {
                name = "Heroku",
                description = "Heroku is a cloud platform as a service supporting several programming languages. One of the first cloud platforms, Heroku has been in development since June 2007, when it supported only the Ruby programming language, but now supports Java, Node.js, Scala, Clojure, Python, PHP, and Go.",
                created = new DateTime(2020, 11, 20),
                lastModified = new DateTime(2020, 11, 20)
            });

            db.Tags.Add(new Tag()
            {
                name = "Docker",
                description = "Docker is a set of platform as a service products that use OS-level virtualization to deliver software in packages called containers. Containers are isolated from one another and bundle their own software, libraries and configuration files; they can communicate with each other through well-defined channels.",
                created = new DateTime(2020, 11, 20),
                lastModified = new DateTime(2020, 11, 20)
            });

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
                },
                new QuestionTag()
                {
                    questionId = question4.id,
                    tagId = angularTag.id
                },
                new QuestionTag()
                {
                    questionId = question4.id,
                    tagId = observableTag.id
                },
                new QuestionTag()
                {
                    questionId = question4.id,
                    tagId = asyncTag.id
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