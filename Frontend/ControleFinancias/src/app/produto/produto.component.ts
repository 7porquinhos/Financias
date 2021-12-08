import { HttpClient } from '@angular/common/http';
import { Component, OnInit } from '@angular/core';
import { FormBuilder, FormGroup, Validators } from '@angular/forms';
import { ToastrService } from 'ngx-toastr';
import { Produto } from '../_models/Produto';
import { AuthService } from '../_services/auth.service';
import { ConsumerApiService } from '../_services/consumerApi.service';
import { GlobalUrl } from '../_services/globalUrl';

@Component({
  selector: 'app-produto',
  templateUrl: './produto.component.html',
  styleUrls: ['./produto.component.css']
})
export class ProdutoComponent implements OnInit {
  produtoId?: number;
  currentDataHora: any;
  bodyDeletarProduto='';
  modoSalvar = 'post';

  produtos?: Produto[];
  produto?: Produto;

  registerForm?: FormGroup;

  readonly apiURL = new GlobalUrl('Produto');

  constructor(
    private authService: AuthService,
    private consumerAPI: ConsumerApiService<Produto>,
    private http: HttpClient,
    private fb: FormBuilder, 
    private toastr: ToastrService
  ) { }

  ngOnInit() {
    this.getProdutos();
    this.validation();
  }
  openModal(template: any){
    this.registerForm?.reset();
    template.show();
  }

  novoProduto(template: any){
    this.modoSalvar = 'post';
    this.registerForm?.reset();
    this.openModal(template);
  }

  salvarProduto(template: any){
    if(this.modoSalvar == 'post'){
    this.produto = Object.assign({}, this.registerForm?.value);
    this.http.post(`${ this.apiURL._baseURL }`, this.produto).
          subscribe(
          resultado => {
            template.hide();
            this.getProdutos();
            this.toastr.success('Produto adicionado com sucesso');
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
      this.produto = Object.assign({id: this.produto?.Id}, this.registerForm?.value);
      this.http.put(`${ this.apiURL._baseURL }/` + this.produto?.Id ,this.produto).subscribe(
      () => {
        template.hide();
        this.getProdutos();
        this.toastr.success('Atualizado com sucesso!');
      }, error => {
        this.toastr.error(`Erro ao tentar Atualizar: ${error}`);
      }
      );
    }
  }

  abrirModalExcluir(produto: Produto, template: any) {
    this.openModal(template);
    this.produto = produto;
    this.bodyDeletarProduto = `Tem certeza que deseja deletar este Produto: ${this.produto.Id}`;
  }

  excluirProduto(template: any) {
    return this.http.delete(`${ this.apiURL._baseURL }/` + this.produto?.Id).
            subscribe(
            resultado => {
              template.hide();
              this.getProdutos();
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

  getProdutos() {
    
    this.http.get<Produto[]>(`${this.apiURL._baseURL}`).
    subscribe(response => {
        this.produtos = response;
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

  
  editarProduto(produto: Produto, template: any){
    this.modoSalvar = 'put';
    this.registerForm?.reset();
    this.currentDataHora = new Date();
    this.openModal(template);
    this.produto = Object.assign({}, produto);
    this.registerForm?.patchValue(produto);
    this.getProdutos();
  }

  validation(){
    this.registerForm = this.fb.group({
      Id: [''],
      Nome: ['', Validators.required],
      Valor: ['', Validators.required]
    });
  }
}
