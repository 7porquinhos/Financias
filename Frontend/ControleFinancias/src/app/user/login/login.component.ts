import { Component, OnInit } from '@angular/core';
import { Router } from '@angular/router';
import { ToastrService } from 'ngx-toastr';
import { AuthService } from 'src/app/_services/auth.service';

@Component({
  selector: 'app-login',
  templateUrl: './login.component.html',
  styleUrls: ['./login.component.css']
})
export class LoginComponent implements OnInit {

  model: any = {};
  constructor(private authService: AuthService,
              public router: Router,
              private toastr: ToastrService) { }

  ngOnInit() {
    if(localStorage.getItem('token') !== null){
      if(!this.authService.loggedIn()){
        this.router.navigate(['/dashboard']);
      }
      else{
        localStorage.removeItem('token');
        localStorage.removeItem('username');
        this.toastr.show('Acesso Expirado!');
        this.router.navigate(['/user/login']);
      }
    }
  }

  login(){
    this.authService.login(this.model)
    .subscribe(
      () => {
        this.router.navigate(['/dashboard']);
      },err =>{
        this.toastr.error(err.error.Message);
      }
    );
  }
}
