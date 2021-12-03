import { HttpClient } from '@angular/common/http';
import { Component, OnInit } from '@angular/core';
import { FormBuilder, FormGroup, Validators } from '@angular/forms';
import { ToastrService } from 'ngx-toastr';
import { Cliente } from '../_models/Cliente';
import { EnderecoCorreios } from '../_models/EnderecoCorreios';
import { AuthService } from '../_services/auth.service';
import { ConsumerApiService } from '../_services/consumerApi.service';
import { GlobalUrl } from '../_services/globalUrl';

@Component({
  selector: 'app-cliente',
  templateUrl: './cliente.component.html',
  styleUrls: ['./cliente.component.css']
})
export class ClienteComponent implements OnInit {

  clienteId?: number;
  currentDataHora: any;
  bodyDeletarUsuario='';
  modoSalvar = 'post';

  clientes?: Cliente[];
  cliente?: Cliente;
  enderecoCorreios?: EnderecoCorreios;


  registerForm?: FormGroup;

  readonly apiURL = new GlobalUrl('Cliente');

  constructor(
    private authService: AuthService,
    private consumerAPI: ConsumerApiService<Cliente>,
    private http: HttpClient,
    private fb: FormBuilder, 
    private toastr: ToastrService) { }

  ngOnInit() {
    this.getCliente();
    this.validation();
  }

  openModal(template: any){
    this.registerForm?.reset();
    template.show();
  }

  novoCliente(template: any){
    this.modoSalvar = 'post';
    this.registerForm?.reset();
    this.openModal(template);
  }

  salvarCliente(template: any){
    if(this.modoSalvar == 'post'){
    this.cliente = Object.assign({}, this.registerForm?.value);
    this.http.post(`${ this.apiURL._baseURL }`, this.cliente).
          subscribe(
          resultado => {
            template.hide();
            this.getCliente();
            this.toastr.success('Cliente adicionado com sucesso');
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
      this.cliente = Object.assign({id: this.cliente?.Id}, this.registerForm?.value);
      this.http.put(`${ this.apiURL._baseURL }/` + this.cliente?.Id ,this.cliente).subscribe(
      () => {
        template.hide();
        this.getCliente();
        this.toastr.success('Atualizado com sucesso!');
      }, error => {
        this.toastr.error(`Erro ao tentar Atualizar: ${error}`);
      }
      );
    }
  }

  consultarCEP(template: any){
    this.cliente = Object.assign({}, this.registerForm?.value);
    this.enderecoCorreios?.CEP != this.cliente?.CEP;
    this.http.post<EnderecoCorreios>(`${ this.apiURL._baseURL }/Consultar-CEP`, this.cliente).
          subscribe(
          resultado => {
            this.enderecoCorreios = resultado;
            this.cliente!.EnderecoCliente = `${ this.enderecoCorreios?.Endereco} - ${this.enderecoCorreios?.Bairro}`;
            this.cliente!.Cidade = this.enderecoCorreios?.Cidade;
            this.carregaEndereco(this.cliente!, template);
            this.toastr.success("CEP Consultado");
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

  abrirModalExcluir(cliente: Cliente, template: any) {
    this.openModal(template);
    this.cliente = cliente;
    this.bodyDeletarUsuario = `Tem certeza que deseja deletar este Usuario: ${cliente.Id}`;
  }

  excluirCliente(template: any) {
    return this.http.delete(`${ this.apiURL._baseURL }/` + this.cliente?.Id).
            subscribe(
            resultado => {
              template.hide();
              this.getCliente();
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

  getCliente() {
    
    this.http.get<Cliente[]>(`${this.apiURL._baseURL}`).
    subscribe(response => {
      this.clientes = response;
    },
    err => {
      if(!this.authService.loggedIn()){
        switch(err.status) {
          case 400:
            this.toastr.error("Bad Request: Solicitação Inválida");
          break;
          case 401:
            this.toastr.error('Unauthorized: Não autorizado');
          break;
          case 404:
            this.toastr.error('Not Found:	Não encontrado');
          break;
        }
      }
      
        
    });
  }

  carregaEndereco(cliente: Cliente, template: any){
    this.registerForm?.reset();
    this.currentDataHora = new Date();
    this.openModal(template);
    this.cliente = Object.assign({}, cliente);
    this.registerForm?.patchValue(cliente);
    this.getCliente();
  }


  editarUsuario(cliente: Cliente, template: any){
    this.modoSalvar = 'put';
    this.registerForm?.reset();
    this.currentDataHora = new Date();
    this.openModal(template);
    this.cliente = Object.assign({}, cliente);
    this.registerForm?.patchValue(cliente);
    this.getCliente();
  }

  validation(){
    this.registerForm = this.fb.group({
      Id: [''],
      Nome: ['', Validators.required],
      Telefone: ['', Validators.required],
      Email: ['', Validators.required],
      CEP: ['', Validators.required],
      EnderecoCliente: ['', Validators.required],
      Cidade: ['', Validators.required]
    });
  }
}
