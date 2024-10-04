import { Component } from '@angular/core';
import { TextColorDirective, CardComponent, CardHeaderComponent, CardBodyComponent, AlertComponent, ContainerComponent, RowComponent, ColComponent, CardGroupComponent, 
  FormDirective, InputGroupComponent, InputGroupTextDirective, FormControlDirective, ButtonDirective,
  TableModule, UtilitiesModule } from '@coreui/angular';
import { NgStyle, CommonModule } from '@angular/common';
import { IconDirective } from '@coreui/icons-angular';
import { ActivatedRoute, Router } from '@angular/router';
import { DocumentoService } from 'src/app/services/documento.service';

@Component({
  selector: 'app-aprobacion',
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
    CardGroupComponent,
    ContainerComponent,
    FormDirective,
    InputGroupTextDirective, 
    FormControlDirective, 
    ButtonDirective,
    IconDirective,
    TableModule, 
    UtilitiesModule,
    NgStyle],
  templateUrl: './aprobacion.component.html',
  styleUrl: './aprobacion.component.scss'
})
export class AprobacionComponent {
  errorValidacion: Boolean = false;
  flagExito: Boolean = false;
  flagEnvio: Boolean = false;
  flagLoad: Boolean = false;
  msgError?: string = "";
  listaHeader: any[] = [];
  listaHeader1: any[] = [];
  _rucProveedor: string = "";
  listaPedidos: any = [];
  pagina: number = 1;
  pedido: any = { docNum: "" };
  _docentry: string = "";
  _user: string = "";

  constructor(private _route: ActivatedRoute, private router: Router, private documentoService: DocumentoService){
    // const dataTemp: any = localStorage.getItem("prov");
    try {
      //   const dataTemp: any = sessionStorage.getItem("prov");
      // const dataProv: any = JSON.parse(dataTemp);
      // console.log("prov", dataProv)
      // this._rucProveedor = dataProv.licTradNum;

      this.listaHeader1 = [
        {
          label: "Articulo / Servicio",
          key: "",
          subKey: "",
          customClass: "centrar",
          type: "",
          value: "",
          visible: true
        },
        {
          label: "Inicial",
          key: "",
          subKey: "",
          customClass: "centrar",
          type: "",
          value: "3",
          visible: true
        },        
        {
          label: "Anterior",
          key: "",
          subKey: "",
          customClass: "centrar",
          type: "",
          value: "3",
          visible: true
        },
        {
          label: "Actual",
          key: "",
          subKey: "",
          customClass: "centrar",
          type: "",
          value: "3",
          visible: true
        },
        {
          label: "Acumulado",
          key: "",
          subKey: "",
          customClass: "centrar",
          type: "",
          value: "3",
          visible: true
        },       
        {
          label: "Saldo",
          key: "",
          subKey: "",
          customClass: "centrar",
          type: "",
          value: "3",
          visible: true
        },     
      ];

      this.listaHeader = [
        {
          label: "Código",
          key: "itemCode",
          subKey: "",
          customClass: "",
          type: "",
          value: "",
          visible: true
        },
        {
          label: "Descripción",
          key: "dscription",
          subKey: "",
          customClass: "",
          type: "",
          value: "",
          visible: true
        },
        {
          label: "U.M",
          key: "unitMsr",
          subKey: "",
          customClass: "",
          type: "",
          value: "",
          visible: true
        },
        {
          label: "Cantidad",
          key: "cantInicial",
          subKey: "",
          customClass: "derecha",
          type: "",
          value: "",
          visible: true
        },
        {
          label: "Importe",
          key: "impInicial",
          subKey: "",
          customClass: "derecha",
          type: "",
          value: "",
          visible: true
        },
        {
          label: "%",
          key: "porInicial",
          subKey: "",
          customClass: "derecha",
          type: "",
          value: "",
          visible: true
        },
        {
          label: "Cantidad",
          key: "cantAcumulada",
          subKey: "",
          customClass: "derecha",
          type: "",
          value: "",
          visible: true
        },
        {
          label: "Importe",
          key: "impAcumulada",
          subKey: "",
          customClass: "derecha",
          type: "",
          value: "",
          visible: true
        },
        {
          label: "%",
          key: "porAnterior",
          subKey: "",
          customClass: "derecha",
          type: "",
          value: "",
          visible: true
        },
        {
          label: "Cantidad",
          key: "quantity",
          subKey: "",
          customClass: "derecha",
          type: "",
          value: "",
          visible: true
        },
        {
          label: "Importe",
          key: "lineTotal",
          subKey: "",
          customClass: "derecha",
          type: "",
          value: "",
          visible: true
        },
        {
          label: "%",
          key: "porActual",
          subKey: "",
          customClass: "derecha",
          type: "",
          value: "",
          visible: true
        },
        {
          label: "Cantidad",
          key: "cantActual",
          subKey: "",
          customClass: "derecha",
          type: "",
          value: "",
          visible: true
        },
        {
          label: "Importe",
          key: "impActual",
          subKey: "",
          customClass: "derecha",
          type: "",
          value: "",
          visible: true
        },        
        {
          label: "%",
          key: "porAcumActual",
          subKey: "",
          customClass: "derecha",
          type: "",
          value: "",
          visible: true
        },        
        {
          label: "Cantidad",
          key: "cantSaldo",
          subKey: "",
          customClass: "derecha",
          type: "",
          value: "",
          visible: true
        },
        {
          label: "Importe",
          key: "impSaldo",
          subKey: "",
          customClass: "derecha",
          type: "",
          value: "",
          visible: true
        },        
        {
          label: "%",
          key: "porSaldo",
          subKey: "",
          customClass: "derecha",
          type: "",
          value: "",
          visible: true
        },        
      ];
      
      let docEntry: string | null;
      docEntry = this._route.snapshot.paramMap.get("id") || "";
      this._docentry = docEntry;
      console.log("draft", docEntry);
      let user: string | null;
      user = this._route.snapshot.paramMap.get("user") || "";
      this._user = user;
      console.log(docEntry, user);
      
      this.obtenerPedido(docEntry);
    } catch (error) {
      console.log("----->ERROR<----", error);      
    }    
  }

  async obtenerPedido(id: string){
    await this.documentoService.getConformidadAprId(id).toPromise().then(data => {
      console.log("pedido", data);
      this.pedido = data;
      this.listaPedidos = this.pedido.detallePedido;
    })
  }

  async crearConformidadServicio(){
    const cmbDecision = document.getElementById("cmbDecision") as HTMLSelectElement;
    const txtComentario = document.getElementById("txtComentario") as HTMLInputElement;
    const txtContrasena = document.getElementById("txtContrasena") as HTMLInputElement;
    // console.log("pedido", this.pedido);
    let data: any = { };
    data.docEntry = this.pedido.docEntry;
    data.userReg = this._user;

    if(cmbDecision.value == ""){
      this.msgError = "Debe seleccionar una decisión";
      this.flagExito = true;
      this.flagEnvio = false;
      return;
    }

    data.docStatus = cmbDecision.value;
    data.comments = txtComentario.value;
    data.password = txtContrasena.value;
    this.flagLoad = true;
    await this.documentoService.aprobarConformidad(data).toPromise().then(data => {
      // console.log(data);
      this.msgError = data?.toString()
      this.flagExito = true;
      this.flagEnvio = false;
      this.flagLoad = false;
    });
  }

  downloadFile(code: string) {
    let nombre: string = "";
    let archivo: string = "";
    switch (code)
    {
      case "1":
        nombre = this.pedido.nomArchivo;
        archivo = this.pedido.archivo.toString().replace("\\\\", "||").replace("\\", "|").replace("\\", "|");
        break;
      case "2":
        nombre = this.pedido.nomArchivo2;
        archivo = this.pedido.archivo2.toString().replace("\\\\", "||").replace("\\", "|").replace("\\", "|");
        break;
      case "3":
        nombre = this.pedido.nomArchivo3;
        archivo = this.pedido.archivo3.toString().replace("\\\\", "||").replace("\\", "|").replace("\\", "|");
        break;
    }

    this.documentoService.downloadFile(archivo).toPromise().then(data => {
      // console.log("archivo", data);
      const archivo: string = data || "";
      // this.pdfSrc = archivo;
      // console.log("archivo", this.pdfSrc);
      var byteCharacters = atob(archivo);
      var byteNumbers = new Array(byteCharacters.length);

      for (var i = 0; i < byteCharacters.length; i++) {
          byteNumbers[i] = byteCharacters.charCodeAt(i);
      }

      var byteArray = new Uint8Array(byteNumbers); 
 
      let filename = nombre;  
      let binaryData = [];
      binaryData.push(byteArray);
      
      let downloadLink = document.createElement('a');
      downloadLink.href = window.URL.createObjectURL(
      new Blob(binaryData, { type: 'blob' }));
      downloadLink.setAttribute('download', filename);
      document.body.appendChild(downloadLink);
      downloadLink.click();
      // this.confirmDialog = true;
    })
  }

  btnOk(){
    const btnSend = document.getElementById("btnSend") as HTMLButtonElement;
    // btnSend.disabled = true;
    this.flagExito = false;
    this.router.navigateByUrl('/login', { replaceUrl: true });
  }
}
