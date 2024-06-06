import { Injectable } from '@angular/core';
import {
  HttpRequest,
  HttpHandler,
  HttpEvent,
  HttpInterceptor,
  HttpErrorResponse
} from '@angular/common/http';
import { Observable, catchError, throwError } from 'rxjs';
import { ErrorService } from 'src/app/error.service';

@Injectable()
export class ErrorInterceptor implements HttpInterceptor {

  constructor(private errorService: ErrorService) {}

  intercept(request: HttpRequest<unknown>, next: HttpHandler): Observable<HttpEvent<unknown>> {
    return next.handle(request).pipe(
      catchError((error: HttpErrorResponse) => {
        let errorMessage = 'An unexpected error occurred.';
        if (error) {
          switch (error.status) {
            case 400:
              if (error.status) {
                const modelStateErrors = [];
                for(const key in error.error.errors) {
                  if (error.error.errors[key]) {
                    modelStateErrors.push(error.error.errors[key])
                  }
                }
                errorMessage = modelStateErrors.flat().join(', ');
              } else {
                errorMessage = 'Bad request error';
              }
              break;
              case 500:
                errorMessage = 'Server error. Please try again later.';
                break;
              default:
                errorMessage = 'An unexpected error occurred.';
                break;
          }
          this.errorService.setError(errorMessage);
        }
        return throwError(() => new Error(errorMessage));
      })
    );
  }
}
