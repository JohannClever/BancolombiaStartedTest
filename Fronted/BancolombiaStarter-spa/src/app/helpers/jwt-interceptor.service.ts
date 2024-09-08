import { Injectable } from '@angular/core';
import { HttpInterceptor, HttpRequest, HttpHandler, HttpEvent, HttpHeaders } from '@angular/common/http';
import { Observable } from 'rxjs';
import { AuthenticationService } from '../service/authentication.service';
import { environment } from 'src/environments/environment';
import { StorageService } from '../service/storage.service';


@Injectable()
export class JwtInterceptor implements HttpInterceptor{
private corsHeaders: any;
  constructor(private authenticationService: AuthenticationService) {
    this.corsHeaders = ({
        'Access-Control-Allow-Request-Method': 'GET, POST, PUT, DELETE, OPTIONS',
        'Access-Control-Allow-Origin' : '*',
        'Allow': 'GET, POST, PUT, DELETE, OPTIONS'
      });
   }

  intercept(request: HttpRequest<any>, next: HttpHandler): Observable<HttpEvent<any>> {
    debugger;
    const token = this.authenticationService.getToken();
    console.log(token);
    const isLoggedIn = token !== '';
    const isApiUrl = request.url.startsWith(environment.baseUrl);
    let headers = this.corsHeaders;

    console.log("headers a mirar");
    console.log(isLoggedIn);
    console.log(isApiUrl);
    if (isLoggedIn && isApiUrl) {
        headers['Authorization'] = `Bearer ${token}`; 
    }
    request = request.clone({
        setHeaders: headers
    });
    console.log(headers);
    console.log(request.url);
    console.log(request.body);
    request.url
    return next.handle(request);
}
}
