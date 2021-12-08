import { Cliente } from "./Cliente";
import { Parcela } from "./Parcela";
import { Produto } from "./Produto";

export class Venda {
    constructor(){

    }

    Id: number;
    ValorVenda: number;
    DataVenda: Date;
    Cliente_Id: number;
    Cliente: Cliente;
    Produto_Id: number;
    Produto: Produto;
    Parcelas: Parcela[];
}
