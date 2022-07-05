import { Component, OnInit, TemplateRef } from '@angular/core';
import { Router } from '@angular/router';
import { BsModalRef, BsModalService } from 'ngx-bootstrap/modal';
import { NgxSpinnerService } from 'ngx-spinner';
import { ToastrService } from 'ngx-toastr';
import { Evento } from '@app/models/Evento';
import { EventoService } from '@app/services/evento.service';
import { environment } from 'src/environments/environment';

@Component({
  selector: 'app-evento-lista',
  templateUrl: './evento-lista.component.html',
  styleUrls: ['./evento-lista.component.scss'],
})
export class EventoListaComponent implements OnInit {
  constructor(
    private eventoService: EventoService,
    private modalService: BsModalService,
    private toastr: ToastrService,
    private spinner: NgxSpinnerService,
    private router: Router
  ) {}

  public eventoId: number;
  public modalRef?: BsModalRef;
  public eventos: Evento[] = [];
  private filtroListado = '';
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
    this.carregarEventos();
  }

  public filtrarEventos(filtroLista: string): Evento[] {
    filtroLista = filtroLista.toLocaleLowerCase();

    return this.eventos.filter(
      (e: Evento) =>
        e.tema.toLocaleLowerCase().indexOf(filtroLista) !== -1 ||
        e.local.toLocaleLowerCase().indexOf(filtroLista) !== -1
    );
  }

  public mostraImagem(imageURL: string): string {
    return imageURL != ''
      ? `${environment.apiURL}resources/images/${imageURL}`
      : 'assets/semimagem.jpeg';
  }

  public carregarEventos(): void {
    this.eventoService.getEventos().subscribe({
      next: (eventosResp: Evento[]) => {
        this.eventos = eventosResp;
        this.eventosFiltrados = this.eventos;
      },
      error: (error: any) => {
        this.spinner.hide();
        this.toastr.error('Erro ao carregar eventos.', 'Erro!');
      },
      complete: () => this.spinner.hide(),
    });
  }

  openModal(event: any, template: TemplateRef<any>, eventoId: number): void {
    event.stopPropagation();
    this.eventoId = eventoId;
    this.modalRef = this.modalService.show(template, { class: 'modal-sm' });
  }

  confirm(): void {
    this.modalRef?.hide();
    this.spinner.show();
    this.eventoService
      .deleteEvento(this.eventoId)
      .subscribe(
        (result: any) => {
          console.log(result);
          this.toastr.success('O evento foi excluído.', 'Sucesso!');

          this.carregarEventos();
        },
        (error: any) => {
          console.log(error);
          this.toastr.error(
            `Erro ao tentar deletar o evento #${this.eventoId}`,
            'Erro!'
          );
        }
      )
      .add(() => {
        this.spinner.hide();
      });
  }

  decline(): void {
    this.modalRef?.hide();
  }

  detalheEvento(id: number): void {
    this.router.navigate([`/eventos/detalhe/${id}`]);
  }
}
