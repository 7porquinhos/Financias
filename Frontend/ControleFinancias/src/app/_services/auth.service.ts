import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { JwtHelperService } from '@auth0/angular-jwt';
import { map } from 'rxjs/operators';
import { GlobalUrl } from './globalUrl';

@Injectable({
  providedIn: 'root'
})
export class AuthService {
  
  //baseURL = 'http://192.168.0.121:9011/api/UserLogin';
  //baseURL = 'http://localhost:44323//api/UserLogin';
  baseURL = new GlobalUrl('UserLogin');
  jwtHelper = new JwtHelperService();
  decodedToken: any;

  constructor(private http: HttpClient) {}

  login(model: any){
    return this.http
      .post(this.baseURL._baseURL, model).pipe(
        map((Response: any) => {
          const user = Response;
          if(user){
            localStorage.setItem('token', user.token);
            this.decodedToken = this.jwtHelper.decodeToken(user.token);
            localStorage.setItem('username', this.decodedToken.unique_name);
            //sessionStorage.setItem('username', this.decodedToken.unique_name);
          }
        })
    );
  }

  loggedIn(){
    const token = localStorage.getItem('token')?.toString();
    return this.jwtHelper.isTokenExpired(token);
  }
}
