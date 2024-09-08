import { HttpClient, HttpErrorResponse, HttpHeaders } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { catchError, tap } from 'rxjs/operators';
import { BehaviorSubject, Observable, of } from 'rxjs';
import { environment } from '../../environments/environment.prod';
import { NotificationService } from './notification.service';

@Injectable({
  providedIn: 'root'
})
export class RestService {


  private loadingSubject: BehaviorSubject<boolean> = new BehaviorSubject<boolean>(false);
  public loading$: Observable<boolean> = this.loadingSubject.asObservable();


  constructor(private http: HttpClient) {
  }

  get<T>(url: string, params: any = {}, token: boolean = true, showError: boolean = true): Observable<T> {
    this.loadingSubject.next(true);

    const paramValues = Object.values(params);
    const urlWithParams = this.isSingleParamObject(params) ?
      url + '/' + this.encodeParamValue(paramValues[0]) :
      this.appendParamsToUrl(url, paramValues);
    return this.http.get<T>(urlWithParams)
      .pipe(
        catchError((error: HttpErrorResponse) => {
          if(showError){
            this.errorHandler(error);
          }
          return of(null);
        }),
        tap((result: T) => {
          this.loadingSubject.next(false);
          return of(result);
        }),
      );
  }

  post<T>(url: string, data: any, token: boolean = true, showError: boolean = true): Observable<T> {
    this.loadingSubject.next(true);
    return this.http.post<T>(url, data)
      .pipe(
        catchError((error: HttpErrorResponse) => {
          if(showError){
            this.errorHandler(error);
          }
          return of(null);
        }),
        tap((result: T) => {
          this.loadingSubject.next(false);
          return of(result);
        }),
      );
  }
  put<T>(url: string, data: any, token: boolean = true, showError: boolean = true): Observable<T> {
    this.loadingSubject.next(true);
    return this.http.put<T>(url, data)
      .pipe(
        catchError((error: HttpErrorResponse) => {
          if(showError){
            this.errorHandler(error);
          }
          return of(null);
        }),
        tap((result: T) => {
          this.loadingSubject.next(false);
          return of(result);
        }),
      );
  }

  delete<T>(url: string, params: any = {}, token: boolean = true, showError: boolean = true): Observable<T> {
    this.loadingSubject.next(true);
    const paramValues = Object.values(params);
    const urlWithParams = url + '/' + this.encodeParamValue(paramValues[0])
    return this.http.delete<T>(urlWithParams)
      .pipe(
        catchError((error: HttpErrorResponse) => {
          if (showError) {
            this.errorHandler(error);
          }
          return of(null);
        }),
        tap((result: T) => {
          this.loadingSubject.next(false);
          return of(result);
        }),
      );
  }

  private errorHandler(error: any): void {
    this.loadingSubject.next(false);
    let message: string;
    switch (error.status) {
      case 0:
        message = 'Se ha presentado una excepción no controlada, por favor contacte a soporte técnico.';
        break;
      case 204:
        message = `No se encontraron datos.`;
        break;
      case 400:
        const errorMessages = Object.entries(error.error.errors)
          .map(([field, messages]: [string, string[]]) => `${field}: ${messages.join(", ")}`)
          .join("\n");
        message = `${error.error.title}\nErrors:\n${errorMessages}`;
        break;
      case 401:
        message = `El recurso solicitado requiere iniciar sesion.`;
        break;
      case 403:
        message = `No tiene permisos para acceder a este recurso.`;
        break;
      case 404:
        message = error.error.detail;
        break;
      case 500:
        message = error.error.title;
        break;
      default:
        message = 'Se ha presentado una excepción no controlada, por favor contacte a soporte técnico.';
        break;
    }

  }
  
  private isSingleParamObject(obj: any): boolean {
    return Object.keys(obj).length === 1;
  }

  private appendParamsToUrl(url: string, params: any[]): string {
    const segments = params.map(param => this.encodeParamValue(param)).join('/');
    return url + '/' + segments;
  }

  private encodeParamValue(param: any): string {
    return encodeURIComponent(param);
  }

}