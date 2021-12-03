import { NgModule } from '@angular/core';
import { RouterModule, Routes } from '@angular/router';
import { AuthGuard } from './auth/auth.guard';
import { ClienteComponent } from './cliente/cliente.component';
import { DashboardComponent } from './dashboard/dashboard.component';
import { LoginComponent } from './user/login/login.component';
import { UserComponent } from './user/user.component';
import { UsuarioComponent } from './usuario/usuario.component';

const routes: Routes = [
    {path: 'user', component: UserComponent,
    children:
    [
      {path: 'login', component: LoginComponent}
    ]
    }
   ,{path: 'dashboard', component: DashboardComponent, canActivate: [AuthGuard]}
   ,{path: 'cliente', component: ClienteComponent, canActivate: [AuthGuard]}
   ,{path: 'usuario', component: UsuarioComponent, canActivate: [AuthGuard]}
   ,{path: '', redirectTo: 'dashboard', pathMatch: 'full'}
   ,{path: '**', redirectTo: 'dashboard', pathMatch: 'full'}
];

@NgModule({
  imports: [RouterModule.forRoot(routes)],
  exports: [RouterModule]
})
export class AppRoutingModule { }
