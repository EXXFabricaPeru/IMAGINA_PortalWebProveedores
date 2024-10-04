import { Component } from '@angular/core';
import { ContainerComponent, RowComponent, ColComponent, TextColorDirective, CardComponent, CardBodyComponent, FormDirective, InputGroupComponent, InputGroupTextDirective, 
         FormControlDirective, ButtonDirective, AlertComponent, CardHeaderComponent } from '@coreui/angular';
import { NgStyle, CommonModule } from '@angular/common';
import { IconDirective } from '@coreui/icons-angular';
import { ActivatedRoute, Router } from '@angular/router';
import { DocumentoService } from 'src/app/services/documento.service';
import { DomSanitizer } from '@angular/platform-browser';

@Component({
  selector: 'app-proveedor',
  standalone: true,
  imports: [TextColorDirective,
    CardComponent,
    CardHeaderComponent,
    CardBodyComponent,
    CommonModule,
    AlertComponent,
    RowComponent,
    ColComponent,
    InputGroupComponent,
    ContainerComponent,
    FormDirective,
    InputGroupTextDirective, 
    FormControlDirective, 
    ButtonDirective,
    IconDirective,
    NgStyle],
  templateUrl: './proveedor.component.html',
  styleUrl: './proveedor.component.scss'
})
export class ProveedorComponent {
  errorValidacion: Boolean = false;
  flagExito: Boolean = false;
  msgError?: string = "";
  listaHeader: any[] = [];
  _rucProveedor: string = "";
  listaPedidos: any = [];
  pagina: number = 1;
  pedido: any;  
  userProv: string = "";

  archivo1: any;
  nomArchivo1: string = "";
  archivo2: any;
  nomArchivo2: string = "";
  archivo3: any;
  nomArchivo3: string = "";
  archivo4: any;
  nomArchivo4: string = "";
  archivo5: any;
  nomArchivo5: string = "";
}
