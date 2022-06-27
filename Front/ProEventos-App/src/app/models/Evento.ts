import { Lote } from "./Lote";
import { Palestrante } from "./Palestrante";
import { RedeSocial } from "./RedeSocial";

export interface Evento {
   id:  number;
   local:  string;
   dataEvento?:  Date;
   tema:  string;
   qtdPessoas:  number;
   lotes:  Lote[];
   redesSociais:  RedeSocial[];
   imageURL:  string;
   telefone:  string;
   email:  string;
   palestrantesEventos:  Palestrante[];

}
