import { HttpClient, HttpHeaders } from '@angular/common/http';
import { Component, OnInit } from '@angular/core';
import { FormBuilder, FormGroup, Validators } from '@angular/forms';
import { ToastrService } from 'ngx-toastr';
import { Observable } from 'rxjs';
import { catchError, retry } from 'rxjs/operators';
import { Usuario } from '../_models/Usuario';
import { ConsumerApiService } from '../_services/consumerApi.service';
import { GlobalUrl } from '../_services/globalUrl';

@Component({
  selector: 'app-usuario',
  templateUrl: './usuario.component.html',
  styleUrls: ['./usuario.component.css']
})
export class UsuarioComponent implements OnInit {

  usuarioId?: number;
  currentDataHora: any;
  bodyDeletarUsuario='';
  modoSalvar = 'post';

  usuarios?: Usuario[];
  usuario?: Usuario;


  registerForm?: FormGroup;

  readonly apiURL = new GlobalUrl('Usuario');
  constructor(private consumerAPI: ConsumerApiService<Usuario>,
    private http: HttpClient,
    private fb: FormBuilder, 
    private toastr: ToastrService) 
  { 

  }

  ngOnInit() {
    this.getUsuario();
    this.validation();
  }

  // Headers
  httpOptions = {
    headers: new HttpHeaders(
      { 
        'Content-Type': 'application/json',
        'Sec-Fetch-Mode': 'cors'
      })
  }

  openModal(template: any){
    this.registerForm?.reset();
    template.show();
  }

  novoUsuario(template: any){
    this.modoSalvar = 'post';
    this.registerForm?.reset();
    this.openModal(template);
  }

  salvarUsuario(template: any){
    if(this.modoSalvar == 'post'){
    this.usuario = Object.assign({}, this.registerForm?.value);
    this.http.post(`${ this.apiURL._baseURL }`, this.usuario).
          subscribe(
          resultado => {
            template.hide();
            this.getUsuario();
            this.toastr.success('Usuario adicionado com sucesso');
          },
          erro => {
            switch(erro.status) {
              case 400:
                this.toastr.error(erro.error.mensagem);
              break;
              case 404:
                this.toastr.error('Erro para salvar.');
              break;
            }
          }
        );
    }
    else{
      this.usuario = Object.assign({id: this.usuario?.Id}, this.registerForm?.value);
      this.http.put(`${ this.apiURL._baseURL }/` + this.usuario?.Id ,this.usuario).subscribe(
      () => {
        template.hide();
        this.getUsuario();
        this.toastr.success('Atualizado com sucesso!');
      }, error => {
        this.toastr.error(`Erro ao tentar Atualizar: ${error}`);
      }
      );
    }
  }

  abrirModalExcluir(usuario: Usuario, template: any) {
    this.openModal(template);
    this.usuario = usuario;
    this.bodyDeletarUsuario = `Tem certeza que deseja deletar este Usuario: ${usuario.Id}`;
  }

  excluirUsuario(template: any) {
    return this.http.delete(`${ this.apiURL._baseURL }/` + this.usuario?.Id).
            subscribe(
            resultado => {
              template.hide();
              this.getUsuario();
              this.toastr.success('Excluído com sucesso!');
            },
            erro => {
              switch(erro.status) {
                case 400:
                  this.toastr.error(erro.error.mensagem);
                break;
                case 404:
                  this.toastr.error('Erro para Excluir!' + erro.error.mensagem);
                break;
              }
            }
          );
  }

  getUsuario() {
    
    this.http.get<Usuario[]>(`${this.apiURL._baseURL}`).
    subscribe(response => {
      console.log(response);
        this.usuarios = response;
    },
    err => {
        this.toastr.error("Error occured.");
    });
  }

   

  editarUsuario(usuario: Usuario, template: any){
    this.modoSalvar = 'put';
    this.registerForm?.reset();
    this.currentDataHora = new Date();
    this.openModal(template);
    this.usuario = Object.assign({}, usuario);
    this.registerForm?.patchValue(usuario);
    this.getUsuario();
  }

  validation(){
    this.registerForm = this.fb.group({
      Id: [''],
      Nome: ['', Validators.required],
      Telefone: ['', Validators.required],
      Email: ['', Validators.required],
      Senha: ['', Validators.required]
    });
  }
}
