import { Cliente } from "./Cliente";
import { Produto } from "./Produto";
import { Venda } from "./Venda";

export class Parcela {
    constructor(){

    }
        Id: number;
        NumParcela: string;
        ValorParcela: number;
        Reembolso: number;
        DataRecebimento: Date;
        Desconto: number;
        DescontoMulta: number;
        DataCompetencia: Date;
        ValorPendente: string;
        Cliente_Id: number;
        Cliente: Cliente;
        Venda_Id: number;
        Venda: Venda;
        Produto_Id: number;
        Produto: Produto;
}
