
import { Component, OnInit, TemplateRef } from '@angular/core';

import { BsModalRef, BsModalService } from 'ngx-bootstrap/modal';
import { NgxSpinnerService } from 'ngx-spinner';
import { ToastrService } from 'ngx-toastr';

import { Evento } from '../../models/Evento';
import { EventoService } from '../../services/evento.service';

@Component({
  selector: 'app-eventos',
  templateUrl: './eventos.component.html',
  styleUrls: ['./eventos.component.scss'],
})
export class EventosComponent implements OnInit {
  constructor(private eventoService: EventoService,
    private modalService: BsModalService,
    private toastr: ToastrService,
    private spinner:NgxSpinnerService
   ) {}

  modalRef?:BsModalRef;
  public eventos: Evento[] = [];
  private filtroListado =  '';
  public eventosFiltrados: Evento[] = [];

  public hideImg = true;
  public widthImg = 150;
  public marginImg = 1;


  public get filtroLista(): string {
    return this.filtroListado;
  }

  public set filtroLista(filtroLista: string) {
    this.filtroListado = filtroLista;
    this.eventosFiltrados = this.filtroLista
      ? this.filtrarEventos(this.filtroLista)
      : this.eventos;

  }

  public ngOnInit() {

    this.spinner.show();
    this.getEventos();
  }

  public filtrarEventos(filtroLista: string): Evento[] {
    filtroLista = filtroLista.toLocaleLowerCase();

    return this.eventos.filter(
      (e: Evento) =>
        e.tema.toLocaleLowerCase().indexOf(filtroLista) !== -1 ||
        e.local.toLocaleLowerCase().indexOf(filtroLista) !== -1
    );
  }

  public getEventos(): void {
    this.eventoService.getEventos().subscribe({
      next: (eventosResp: Evento[]) => {
        this.eventos = eventosResp;
        this.eventosFiltrados = this.eventos;
      },
      error: (error:any) => {this.spinner.hide();
                            this.toastr.error('Erro ao carregar eventos.', 'Erro!')},
      complete: () => this.spinner.hide()
    });
  }

  openModal(template: TemplateRef<any>):void {
    this.modalRef = this.modalService.show(template, {class: 'modal-sm'});
  }

  confirm(): void {
    this.showSuccess();
    this.modalRef?.hide();
  }

  decline(): void {
    this.modalRef?.hide();
  }

  showSuccess():void {
    this.toastr.success('O Evento foi excluído.', 'Sucesso!');
  }
}