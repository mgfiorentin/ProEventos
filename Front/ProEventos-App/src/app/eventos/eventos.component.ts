import { HttpClient } from '@angular/common/http';
import { Component, OnInit } from '@angular/core';

@Component({
  selector: 'app-eventos',
  templateUrl: './eventos.component.html',
  styleUrls: ['./eventos.component.scss']
})
export class EventosComponent implements OnInit {
  hideImg = true;
  public eventos: any;
  widthImg = 150;
  marginImg = 1;
  private _filtroLista: string= '';
  public eventosFiltrados:any;

  public get filtroLista():string {return this._filtroLista}

  public set filtroLista(filtroLista:string ){
    this._filtroLista=filtroLista;
    this.eventosFiltrados = this.filtroLista ? this.filtrarEventos(this.filtroLista) :this.eventos
  }

  constructor(private http: HttpClient) { }

  ngOnInit() {
    this.getEventos();

  }

  public filtrarEventos(filtroLista:string): any {
    filtroLista = filtroLista.toLocaleLowerCase();

    return this.eventos.filter(
      (e:any)  => e.tema.toLocaleLowerCase().indexOf(filtroLista) !== -1 ||
      e.local.toLocaleLowerCase().indexOf(filtroLista)!== -1
    );
  }

public getEventos(): void {

  this.http.get('https://localhost:5001/api/Eventos/')
  .subscribe(response => {
    this.eventos = response;
    this.eventosFiltrados = this.eventos;
  },
    error => console.log(error));



}

}
