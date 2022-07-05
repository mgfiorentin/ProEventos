import { i18nMetaToJSDoc } from '@angular/compiler/src/render3/view/i18n/meta';
import { Component, OnInit, TemplateRef } from '@angular/core';
import {
  AbstractControl,
  FormArray,
  FormBuilder,
  FormControl,
  FormGroup,
  Validators,
} from '@angular/forms';
import { ActivatedRoute, Router } from '@angular/router';
import { Evento } from '@app/models/Evento';
import { Lote } from '@app/models/Lote';
import { EventoService } from '@app/services/evento.service';
import { LoteService } from '@app/services/lote.service';
import { BsLocaleService } from 'ngx-bootstrap/datepicker';
import { BsModalRef, BsModalService } from 'ngx-bootstrap/modal';
import { NgxSpinner, NgxSpinnerService } from 'ngx-spinner';
import { ToastrService } from 'ngx-toastr';
import { environment } from 'src/environments/environment';

@Component({
  selector: 'app-evento-detalhe',
  templateUrl: './evento-detalhe.component.html',
  styleUrls: ['./evento-detalhe.component.scss'],
})
export class EventoDetalheComponent implements OnInit {
  form: FormGroup;
  locale = 'pt-br';
  evento = {} as Evento;
  debug: any;
  estadoSalvar: any = 'post';
  eventoId: number;
  public modalRef?: BsModalRef;
  loteAtual = { id: 0, nome: '', indice: 0 };
  imagemURL = 'assets/upload.png';
  file: File;

  get lotes(): FormArray {
    return this.form.get('lotes') as FormArray;
  }

  get f(): any {
    return this.form.controls;
  }

  get modoEditar(): boolean {
    return this.estadoSalvar == 'put';
  }

  constructor(
    private fb: FormBuilder,
    private localeService: BsLocaleService,
    private activatedRouter: ActivatedRoute,
    private eventoService: EventoService,
    private spinner: NgxSpinnerService,
    private toastr: ToastrService,
    private router: Router,
    private loteService: LoteService,
    private modalService: BsModalService
  ) {
    this.localeService.use(this.locale);
  }

  ngOnInit() {
    this.carregarEvento();
    this.validation();
  }

  public validation(): void {
    this.form = this.fb.group({
      tema: [
        '',
        [
          Validators.required,
          Validators.minLength(4),
          Validators.maxLength(50),
        ],
      ],
      local: ['', Validators.required],
      dataEvento: ['', Validators.required],
      qtdPessoas: [
        '',
        [Validators.required, Validators.min(0), Validators.max(120000)],
      ],
      telefone: ['', Validators.required],
      email: ['', [Validators.required, Validators.email]],
      imageURL: [''],
      lotes: this.fb.array([]),
    });
  }

  adicionarLote(): void {
    this.lotes.push(this.criarLote({ id: 0 } as Lote));
  }

  criarLote(lote: Lote): FormGroup {
    return this.fb.group({
      id: [lote.id],
      nome: [lote.nome, Validators.required],
      preco: [lote.preco, Validators.required],
      quantidade: [lote.quantidade, Validators.required],
      dataInicio: [lote.dataInicio],
      dataFim: [lote.dataFim],
    });
  }

  resetForm(): void {
    this.form.reset();
  }

  public cssValidator(campoForm: FormControl | AbstractControl): any {
    return { 'is-invalid': campoForm.errors && campoForm.touched };
  }

  public carregarEvento(): void {
    this.eventoId = +this.activatedRouter.snapshot.paramMap.get('id');

    if (this.eventoId != null && this.eventoId != 0) {
      this.estadoSalvar = 'put';

      this.eventoService
        .getEventoById(this.eventoId)
        .subscribe({
          next: (evento: Evento) => {
            this.evento = { ...evento };
            this.form.patchValue(this.evento);
            if (this.evento.imageURL != '' ){
              this.imagemURL=environment.apiURL + 'resources/images/' + this.evento.imageURL;
            }
            this.evento.lotes.forEach((lote) => {
              this.lotes.push(this.criarLote(lote));
            });
            //this.carregarLotes();
          },
          error: (error: any) => {
            console.error(error);
            this.toastr.error('Erro ao carregar evento.', 'Erro!');
          },
          complete: () => {},
        })
        .add(() => this.spinner.hide());
    }
  }

  /*
  public carregarLotes(): void {
    this.loteService
      .getLotesByEventoId(this.eventoId)
      .subscribe(
        (lotesRetorno: Lote[]) => {
          lotesRetorno.forEach((lote) => {
            this.lotes.push(this.criarLote(lote));
          });
        },

        (error: any) => {
          console.error(error);
          this.toastr.error('Erro ao tentar carregar lotes', 'Erro!');
        }
      )
      .add(() => this.spinner.hide());
  }*/

  public salvarEvento(): void {
    this.spinner.show();

    if (this.form.valid) {
      this.evento =
        this.estadoSalvar == 'post'
          ? (this.evento = { ...this.form.value })
          : (this.evento = { id: this.evento.id, ...this.form.value });

      this.eventoService[this.estadoSalvar](this.evento).subscribe(
        (eventoRetorno: Evento) => {
          this.toastr.success('Evento salvo.', 'Sucesso!');
          this.router.navigate([`eventos/detalhe/${eventoRetorno.id}`]);
        },
        (error: any) => {
          console.error(error);
          this.spinner.hide();
          this.toastr.error('Erro ao salvar evento.', 'Erro!');
        },
        () => {
          this.spinner.hide();
        }
      );
    }
  }

  public retornaTituloLote(titulo: string): string {
    return titulo == null || titulo == '' ? 'Nome do lote' : titulo;
  }

  public salvarLotes(): void {
    if (this.form.controls.lotes.valid) {
      this.spinner.show();
      this.loteService
        .saveLote(this.eventoId, this.form.value.lotes)
        .subscribe(
          () => {
            this.toastr.success('Lote salvo com sucesso.', 'Sucesso!');
            //this.lotes.reset();
          },
          (error: any) => {
            console.error(error);
            this.toastr.error('Erro ao salvar lote.', 'Erro!');
          }
        )
        .add(() => this.spinner.hide());
    }
  }

  public removerLote(template: TemplateRef<any>, indice: number): void {
    this.loteAtual.id = this.lotes.get(indice + '.id').value;
    this.loteAtual.nome = this.lotes.get(indice + '.nome').value;
    this.loteAtual.indice = indice;

    this.modalRef = this.modalService.show(template, { class: 'modal-sm' });

    //
  }

  public confirm(): void {
    this.modalRef?.hide();
    this.spinner.show();

    this.loteService
      .deleteLote(this.eventoId, this.loteAtual.id)
      .subscribe(
        () => {
          this.toastr.success(
            `Lote #${this.loteAtual.id}excluído com sucesso.`,
            'Sucesso!'
          );
          this.lotes.removeAt(this.loteAtual.indice);
        },
        (error: any) => {
          this.toastr.error(
            `Erro ao excluir lote #${this.loteAtual.id}`,
            'Erro!'
          );
          console.error(error);
        }
      )
      .add(() => this.spinner.hide());
  }

  public decline(): void {
    this.modalRef?.hide();
  }

  onFileChange(ev: any): void {
    const reader = new FileReader();
    reader.onload = (event: any) => this.imagemURL = event.target.result;
    this.file = ev.target.files;
    reader.readAsDataURL(this.file[0]);
    this.uploadImagem();
  }

  uploadImagem(): void {
    this.spinner.show();
    this.eventoService
      .postUpload(this.eventoId, this.file)
      .subscribe(
        () => {
          this.carregarEvento();
          this.toastr.success('Imagem atualizada com sucesso.', 'Sucesso!');
        },
        (error: any) => {
          console.error(error);
          this.toastr.error('Erro durante upload de imagem.', 'Erro!');
        }
      )
      .add(() => this.spinner.hide());
  }
}
