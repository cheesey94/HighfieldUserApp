import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { Observable } from 'rxjs';

@Injectable({
  providedIn: 'root'
})
export class UserService {

  private baseUrl = 'https://localhost:5001/api/';

  constructor(private http: HttpClient) { }

  getTopColours() : Observable<any> {
    const url = `${this.baseUrl}users/top-colours`;
    return this.http.get(url);
  }

  getAgePlusTwenty(): Observable<any> {
    const url = `${this.baseUrl}users/age-plus-twenty`;
    return this.http.get(url);
  }
}
