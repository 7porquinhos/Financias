import { HttpClient } from '@angular/common/http';
import { Component, OnInit } from '@angular/core';
import { FormBuilder, FormGroup, Validators } from '@angular/forms';
import { ToastrService } from 'ngx-toastr';
import { Venda } from '../_models/Venda';
import { AuthService } from '../_services/auth.service';
import { ConsumerApiService } from '../_services/consumerApi.service';
import { GlobalUrl } from '../_services/globalUrl';

@Component({
  selector: 'app-venda',
  templateUrl: './venda.component.html',
  styleUrls: ['./venda.component.css']
})
export class VendaComponent implements OnInit {

  VendaId?: number;
  currentDataHora: any;
  bodyDeletarVenda='';
  modoSalvar = 'post';

  vendas?: Venda[];
  venda?: Venda;

  registerForm?: FormGroup;

  readonly apiURL = new GlobalUrl('Venda');

  constructor(
    private authService: AuthService,
    private consumerAPI: ConsumerApiService<Venda>,
    private http: HttpClient,
    private fb: FormBuilder, 
    private toastr: ToastrService
  ) { }

  ngOnInit() {
    this.getVenda();
    this.validation();
  }
  openModal(template: any){
    this.registerForm?.reset();
    template.show();
  }

  novaVenda(template: any){
    this.modoSalvar = 'post';
    this.registerForm?.reset();
    this.openModal(template);
  }

  salvarVenda(template: any){
    if(this.modoSalvar == 'post'){
    this.venda = Object.assign({}, this.registerForm?.value);
    this.http.post(`${ this.apiURL._baseURL }`, this.venda).
          subscribe(
          resultado => {
            template.hide();
            this.getVenda();
            this.toastr.success('Venda adicionado com sucesso');
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
      this.venda = Object.assign({id: this.venda?.Id}, this.registerForm?.value);
      this.http.put(`${ this.apiURL._baseURL }/` + this.venda?.Id ,this.venda).subscribe(
      () => {
        template.hide();
        this.getVenda();
        this.toastr.success('Atualizado com sucesso!');
      }, error => {
        this.toastr.error(`Erro ao tentar Atualizar: ${error}`);
      }
      );
    }
  }

  abrirModalExcluir(venda: Venda, template: any) {
    this.openModal(template);
    this.venda = venda;
    this.bodyDeletarVenda = `Tem certeza que deseja deletar este Produto: ${this.venda.Id}`;
  }

  excluirProduto(template: any) {
    return this.http.delete(`${ this.apiURL._baseURL }/` + this.venda?.Id).
            subscribe(
            resultado => {
              template.hide();
              this.getVenda();
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

  getVenda() {
    
    this.http.get<Venda[]>(`${this.apiURL._baseURL}`).
    subscribe(response => {
        this.vendas = response;
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

  
  editarProduto(venda: Venda, template: any){
    this.modoSalvar = 'put';
    this.registerForm?.reset();
    this.currentDataHora = new Date();
    this.openModal(template);
    this.venda = Object.assign({}, venda);
    this.registerForm?.patchValue(venda);
    this.getVenda();
  }

  validation(){
    this.registerForm = this.fb.group({
      Id: [''],
      ValorVenda: ['', Validators.required]
    });
  }
}
