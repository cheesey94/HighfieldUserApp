import { Component, OnInit} from '@angular/core';
import { UserService } from './user.service';
import { ErrorService } from './error.service';

@Component({
  selector: 'app-root',
  templateUrl: './app.component.html',
  styleUrls: ['./app.component.css'],
})
export class AppComponent implements OnInit {
  title = 'User Informaton';
  active = 1;

  users: any[] = [];
  topColours: any[] = [];
  agesPlusTwenty: any[] = [];
  errorMessage: string | null = null;

  constructor(private userService: UserService, private errorService: ErrorService) { }
  ngOnInit(): void {
    this.fetchTopColours();
    this.fetchAgePlusTwenty();

    this.errorService.getError().subscribe(message => {
      this.errorMessage = message;
    })
  }
  
  fetchTopColours(): void {
    this.userService.getTopColours().subscribe({
      next: response => this.topColours = response,
      error: error => console.log(error),
      complete: () => console.log('Top colours request has completed')
    });
  }
  fetchAgePlusTwenty(): void {
    this.userService.getAgePlusTwenty().subscribe({
      next: response => this.agesPlusTwenty = response,
      error: error => console.log(error),
      complete: () => console.log('Age plus 20 request has completed')
    });
  }

  
}