import { Injectable } from '@angular/core';
import { BehaviorSubject, Observable } from 'rxjs';

@Injectable({
  providedIn: 'root'
})
export class ErrorService {
  private errorSubject = new BehaviorSubject<string | null>(null);

  setError(message: string): void {
    this.errorSubject.next(message);
  }

  clearError(): void {
    this.errorSubject.next(null);
  }

  getError(): Observable<string | null> {
    return this.errorSubject.asObservable();
  }
}