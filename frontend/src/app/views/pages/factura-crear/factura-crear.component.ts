import { Component } from '@angular/core';
import { TextColorDirective, CardComponent, CardHeaderComponent, CardBodyComponent, AlertComponent, ContainerComponent, RowComponent, ColComponent, CardGroupComponent,
  FormDirective, InputGroupComponent, InputGroupTextDirective, FormControlDirective, ButtonDirective,
  TableModule, UtilitiesModule } from '@coreui/angular';
import { NgStyle, CommonModule } from '@angular/common';
import { IconDirective } from '@coreui/icons-angular';
import { ActivatedRoute, Router } from '@angular/router';
import { DomSanitizer } from '@angular/platform-browser';
import { DocumentoService } from 'src/app/services/documento.service';
import { MaestroService } from 'src/app/services/maestro.service';
import { ProveedorService } from 'src/app/services/proveedor.service';
import * as converter from 'xml-js';

@Component({
  selector: 'app-factura-crear',
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
  templateUrl: './factura-crear.component.html',
  styleUrl: './factura-crear.component.scss'
})
export class FacturaCrearComponent {
  errorValidacion: Boolean = false;
  flagExito: Boolean = false;
  flagError: Boolean = false;
  modalDialog: Boolean = false;
  aplicaFactoring: Boolean = false;
  flagLoad: Boolean = false;

  msgError?: string = "";
  listaHeader: any[] = [];
  listaHeaderDocs: any[] = [];
  _rucProveedor: string = "";
  listaPedidos: any = [];
  listaPedidosAux: any = [];
  listaCS: any = [];
  listaProvFact: any = [];

  pagina: number = 1;
  pedido: any = {docStatus: ""};
  userProv: string = "";
  
  inputXml : any;
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

  idSucursal: number = 0;

  constructor(private _route: ActivatedRoute, private router: Router, private documentoService: DocumentoService,  private maestroService: MaestroService, proveedorService: ProveedorService, private sanitizer: DomSanitizer){
    // const dataTemp: any = localStorage.getItem("prov");
    const dataTemp: any = sessionStorage.getItem("prov");
    const dataProv: any = JSON.parse(dataTemp);
    // console.log("prov", dataProv)
    this._rucProveedor = dataProv.licTradNum;
    this.userProv = dataProv.licTradNum;

    this.listaHeader = [
      // {
      //   label: "Código",
      //   key: "itemCode",
      //   subKey: "",
      //   customClass: "",
      //   type: "",
      //   value: "",
      //   visible: true
      // },
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
        label: "Cantidad",
        key: "quantity",
        subKey: "",
        customClass: "derecha",
        type: "",
        value: "",
        visible: true
      },
      // {
      //   label: "U.M",
      //   key: "unitMsr",
      //   subKey: "",
      //   customClass: "",
      //   type: "",
      //   value: "",
      //   visible: true
      // },
      // {
      //   label: "Fec. Entrega",
      //   key: "shipDate",
      //   subKey: "",
      //   customClass: "fecha",
      //   type: "",
      //   value: "",
      //   visible: true
      // },
      // {
      //   label: "Impuesto",
      //   key: "taxCode",
      //   subKey: "",
      //   customClass: "",
      //   type: "",
      //   value: "",
      //   visible: true
      // },
      {
        label: "Prec. Unit",
        key: "price",
        subKey: "",
        customClass: "derecha",
        type: "",
        value: "",
        visible: true
      },
      {
        label: "Precio Total",
        key: "lineTotal",
        subKey: "",
        customClass: "derecha",
        type: "",
        value: "",
        visible: true
      },
    ];

    this.listaHeaderDocs = [
      {
        label: "*",
        key: "docEntry",
        subKey: "",
        customClass: "",
        type: "check",
        value: "",
        visible: true
      },
      {
        label: "Nro",
        key: "docNum",
        subKey: "",
        customClass: "",
        type: "",
        value: "",
        visible: true
      },
      {
        label: "NroGuia",
        key: "numAtCard",
        subKey: "",
        customClass: "",
        type: "",
        value: "",
        visible: true
      },
      {
        label: "NroOC",
        key: "numeroCotizacion",
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
        label: "Cantidad",
        key: "quantity",
        subKey: "",
        customClass: "derecha",
        type: "",
        value: "",
        visible: true
      },
      {
        label: "PrecioTotal",
        key: "lineTotal",
        subKey: "",
        customClass: "derecha",
        type: "",
        value: "",
        visible: true
      },
      {
        label: "Fec. Entrega",
        key: "docDueDate",
        subKey: "",
        customClass: "fecha",
        type: "",
        value: "",
        visible: true
      },
      {
        label: "Sucursal",
        key: "sucursal",
        subKey: "",
        customClass: "",
        type: "",
        value: "",
        visible: true
      },
    ];

    let docEntry: string | null;

    docEntry = this._route.snapshot.paramMap.get("id") || "";


    proveedorService.getListaProveedorFact().toPromise().then(data => {
      this.listaProvFact = data;
      console.log(this.listaProvFact);
    });


    this.obtenerPedido(docEntry);
  }

  async obtenerPedido(id: string){


    await this.documentoService.getConformidadId(id).toPromise().then(data => {
      console.log("pedido", data);
      this.pedido = data;
      this.listaPedidos = this.pedido.detallePedido;
      for(let i = 0; i < this.listaPedidos.length; i++){
        this.listaPedidos[i].sucursal = this.pedido.sucursal;
        this.idSucursal = this.pedido.idSucursal;
      }
    })
  }

  capturarFile(event: any, tipo: string){
    const fileCopy = event.target.files[0];
    // console.log(fileCopy);

    this.extraerBase64(fileCopy).then((file: any) => {
      // console.log(file);

      switch(tipo){
        case "1":
          this.archivo1 = (file.base).split(',')[1];
          this.nomArchivo1 = fileCopy.name;

          this.leerXML(fileCopy);
          break;
        case "2":
          this.archivo2 = (file.base).split(',')[1];
          this.nomArchivo2 = fileCopy.name;
          break;
        case "3":
          this.archivo3 = (file.base).split(',')[1];
          this.nomArchivo3 = fileCopy.name;
          break;
        case "4":
          this.archivo4 = (file.base).split(',')[1];
          this.nomArchivo4 = fileCopy.name;
          break;
        case "5":
          this.archivo5 = (file.base).split(',')[1];
          this.nomArchivo5 = fileCopy.name;
          break;
      }

      // console.log(this.nomArchivo, this.archivo);
      this.msgError = "";
      this.errorValidacion = false;
    })
  }

  extraerBase64 = async ( $event: any ) => new Promise(( resolve, reject) => {
    let database: any;
    try {
      const unsafeFile = window.URL.createObjectURL($event);
      const file = this.sanitizer.bypassSecurityTrustUrl(unsafeFile);
      const reader = new FileReader();
      reader.readAsDataURL($event);

      database = reader.onload = () => {
        resolve({
          base: reader.result
        });
      };

      database = reader.onerror = () => {
        resolve({
          base: null
        });
      };
    } catch (error) {
      database = null
    }

    return database;
  })

  async guardar(){
    const xpta: string = await this.obtenerDatos();
    // console.log("factura", this.pedido);

    const txtSerie = document.getElementById("txtSerie") as HTMLInputElement
    const txtNumer = document.getElementById("txtNumero") as HTMLInputElement
    const txtFecha = document.getElementById("txtFechaDoc") as HTMLInputElement
    const txtTotal = document.getElementById("txtTotal") as HTMLInputElement

    const fd = txtFecha.value;

    // console.log("fecha", fd);

    let dataSunat: any = { }
    dataSunat.numRuc = this.pedido.licTradNum;
    dataSunat.codComp = "01";
    dataSunat.numeroSerie = txtSerie.value;
    dataSunat.numero = txtNumer.value;
    dataSunat.fechaEmision = fd;
    dataSunat.monto = txtTotal.value.replace(",","")

    if(xpta != ""){
      this.msgError = xpta;
      this.errorValidacion = true;
      return;
    }

    // const dataToken: any = await this.documentoService.obtenerTokenSunat().toPromise();
    // console.log(dataToken);
    // console.log(dataSunat);

    // if(dataToken.access_token){
    //   try {
    //     const dataFactura: any = await this.documentoService.validarFactura(dataSunat, dataToken.access_token).toPromise();
    //     switch(dataFactura.data.estadoCp){
    //       case "0":
    //         this.msgError = "Factura no existe en SUNAT"
    //         break;
    //       case "1":
    //         this.msgError = "Aceptada"
    //         break;
    //       case "2":
    //         this.msgError = "Factura anulada"
    //         break;
    //       case "3":
    //         this.msgError = "Autorizada"
    //         break;
    //       case "4":
    //         this.msgError = "Factura no autorizada"
    //         break;
    //     }

    //     if(dataFactura.data.estadoCp != "1" && dataFactura.data.estadoCp != "3"){
    //       this.errorValidacion = true;
    //       this.flagError = true;
    //       return;
    //     }
    //   } catch (error) {
    //     console.log(error)
    //   }
    // }

    this.flagLoad = true;

    const data: any = await this.documentoService.crearFactura(this.pedido).toPromise()
    this.flagLoad = false;

    console.log(data);
    this.msgError = data?.toString();
    this.errorValidacion = false;
    if(this.msgError?.includes("éxito"))
      this.flagExito = true;
    else
      this.flagError = true;

  }

  async obtenerDatos(): Promise<string>{
    let xVal: string = "";
    try {

      if(this.nomArchivo1 == ""){
        xVal = "Tiene que adjuntar el XML";
      }

      if(this.nomArchivo2 == ""){
        xVal = "Tiene que adjuntar el CDR";
      }

      if(this.nomArchivo3 == ""){
        xVal = "Tiene que adjuntar el PDF";
      }

      if(this.pedido.u_EXC_FVCAFI != null)
      {
        if(this.nomArchivo5 == ""){
          xVal = "Tiene que adjuntar la Carta Fianza";
        }
      }
      // if(this.nomArchivo4 == ""){
      //   xVal = "Tiene que adjuntar la Garantia por cumplimiento";
      // }

      this.pedido.userReg = this.userProv;

      this.pedido.archivo = this.archivo1;
      this.pedido.nomArchivo = this.nomArchivo1;

      this.pedido.archivo2 = this.archivo2;
      this.pedido.nomArchivo2 = this.nomArchivo1;

      this.pedido.archivo3 = this.archivo3;
      this.pedido.nomArchivo3 = this.nomArchivo3;

      if(this.archivo4 == undefined)
        this.archivo4 = null;

      if(this.archivo5 == undefined)
        this.archivo5 == null;

      this.pedido.archivo4 = this.archivo4;
      this.pedido.nomArchivo4 = this.nomArchivo4;

      this.pedido.archivo5 = this.archivo5;
      this.pedido.nomArchivo5 = this.nomArchivo5;

      const xSerie = document.getElementById("txtSerie") as HTMLInputElement
      const xNumero = document.getElementById("txtNumero") as HTMLInputElement
      const xAplicaFac = document.getElementById("cmbAplFact") as HTMLSelectElement
      const txtFechaDoc = document.getElementById("txtFechaDoc") as HTMLInputElement
      const txtComentario = document.getElementById("txtComentario") as HTMLInputElement
      const xProveeFac = document.getElementById("cmbProFact") as HTMLSelectElement

      if(xSerie.value == ""){
        xVal = "Tiene que colocar una serie de documento";
      }

      if(xNumero.value == ""){
        xVal = "Tiene que colocar una número de documento";
      }

      let fecha: Date;
      if(txtFechaDoc.value == "")
        xVal = "Falta Fecha de vencimiento"
      //else{
      //   const fd = xVal.split("-");
      //   fecha = new Date(fd[0], f)
      // }

      let xRazonSocial: string = "";

      for(let x=0; x<this.listaProvFact.length; x++){
        if(this.listaProvFact[x].cardCode == xProveeFac.value){
          xRazonSocial = this.listaProvFact[x].cardName;
          break;
        }
      }

      this.pedido.docDate = txtFechaDoc.value;
      this.pedido.folioPref = xSerie.value;
      this.pedido.folioNum = xNumero.value;
      this.pedido.aplicaFactoring = xAplicaFac.value;
      this.pedido.proveedorFactoring = xProveeFac.value;
      this.pedido.rSProveedorFactoring = xRazonSocial;
      this.pedido.comments = txtComentario.value;

      const _table: any = document.querySelector("#gridDoc");
      // console.log(_table);

      for(let i = 1; i < _table.rows.length; i++){
        this.listaPedidos[i - 1].lineTotal = this.listaPedidos[i - 1].lineTotal.toString().replace(",", "");
        this.listaPedidos[i - 1].pendQuantity = this.listaPedidos[i - 1].pendQuantity.toString().replace(",", "");
        this.listaPedidos[i - 1].price = this.listaPedidos[i - 1].price.toString().replace(",", "");
      }

    } catch (error: any) {
      xVal = error.toString();
    }

    return xVal;
  }

  btnOk(){
    console.log("ok");
    
    this.router.navigateByUrl('/factura', { replaceUrl: true });
  }

  openDocuments(){
    this.listaCS = [];
    this.modalDialog = true;
  }

  async buscarConformidad(){
    this.listaCS = [];
    const txtDesde = document.getElementById("txtDesde") as HTMLInputElement;
    const txtHasta = document.getElementById("txtHasta") as HTMLInputElement;

    const fi: string = txtDesde.value.replace("-","").replace("-","");
    const ff: string = txtHasta.value.replace("-","").replace("-","");
    await this.documentoService.getConformidadDisponible(this._rucProveedor, fi, ff, this.idSucursal.toString()).toPromise().then(pedidos => {
      console.log("conformidad->", pedidos);
      this.listaPedidosAux = pedidos;
    });
    console.log("aux", this.listaPedidosAux);

    for(let i = 0; i < this.listaPedidosAux.length; i++){
      let contador: number = 0;

      for(let j = 0; j < this.listaPedidos.length; j++){
        if(this.listaPedidosAux[i].sucursal != this.listaPedidos[j].sucursal)
          continue;

        if(this.listaPedidosAux[i].item.lineNum == this.listaPedidos[j].lineNum &&
          this.listaPedidosAux[i].docEntry == this.listaPedidos[j].docEntry
        ){
            contador++;
        }
      }

      if(contador==0){
        const dato: any = {
          docEntry: this.listaPedidosAux[i].docEntry,
          docNum: this.listaPedidosAux[i].docNum,
          dscription: this.listaPedidosAux[i].item.dscription,
          quantity: this.listaPedidosAux[i].item.quantity,
          lineTotal: this.listaPedidosAux[i].item.lineTotal,
          docDueDate: this.listaPedidosAux[i].docDueDate,
          sucursal: this.listaPedidosAux[i].sucursal,
          lineNum: this.listaPedidosAux[i].item.lineNum,
          taxCode: this.listaPedidosAux[i].item.taxCode,
          numAtCard: this.listaPedidosAux[i].numAtCard,
          numeroCotizacion: this.listaPedidosAux[i].numeroCotizacion
        }
        this.listaCS.push(dato)
      }
    }

    console.log("csD", this.listaCS);

  }

  seleccionarConformidad(){
    const _table: any = document.querySelector("#gridDoc");
    for(let i = 1; i < _table.rows.length; i++){
      const _sel = _table.rows[i].cells[0].children[0] as HTMLInputElement;
      // console.log(_sel.checked);
      // console.log();

      if(_sel.checked){
        const dato: any = {
          docEntry: this.listaCS[i - 1].docEntry,
          docNum: this.listaCS[i - 1].docNum,
          dscription: this.listaCS[i - 1].dscription,
          quantity: this.listaCS[i - 1].quantity,
          lineTotal: this.listaCS[i - 1].lineTotal,
          shipDate: this.listaCS[i - 1].docDueDate,
          sucursal: this.listaCS[i - 1].sucursal,
          lineNum: this.listaCS[i - 1].lineNum,
          price: Number(this.listaCS[i - 1].lineTotal) / Number(this.listaCS[i - 1].quantity),
          taxCode: this.listaCS[i - 1].taxCode
        }
        this.listaPedidos.push(dato);
      }
    }

    let subTotal: number = 0;
    let impuesto: number = 0;
    this.listaPedidos.forEach((item: any) => {
      // console.log(item);
      subTotal += item.lineTotal;
      if(item.taxCode == "IGV"){
        impuesto += Number(item.lineTotal) * 0.18
      }
    });

    this.pedido.docTotal = subTotal + impuesto;
    this.pedido.vatSum = impuesto;

    this.modalDialog = false;
  }

  async aplicaProvFactoring(){
    const cmbAplFact = document.getElementById("cmbAplFact") as HTMLSelectElement
    const cmbProFact = document.getElementById("cmbProFact") as HTMLSelectElement
    if(cmbAplFact.value == "Y"){
      this.aplicaFactoring = true;
      // await this.maestroService.getProveedorFactoring().toPromise().then( data => {
      //   this.listaProvFact = data;
      // });
    }else{
      this.aplicaFactoring = false;
      cmbProFact.selectedIndex = 0;
    }
  }

  sumarDias(){
    // const txtFecha = document.getElementById("txtFechaDoc") as HTMLInputElement
    // const txtFechaEnt = document.getElementById("txtFechaEnt") as HTMLInputElement
    // const fd = txtFecha.value.split("-");
    // const day: string = fd[2];
    // const mes: string = fd[1];
    // const ano: string = fd[0];
    // const fdoc = (new Date(Number(ano), Number(mes), Number(day)));
    // console.log( fdoc );
    // fdoc.setDate(fdoc.getDate() + 30)
    // console.log( fdoc );
    // this.pedido.docDueDate = fdoc

    // console.log(fd, fdoc);
  }

  leerXML(event: any){
    const txtSerie = document.getElementById("txtSerie") as HTMLInputElement
    const txtNumero = document.getElementById("txtNumero") as HTMLInputElement
    const txtFechaDoc = document.getElementById("txtFechaDoc") as HTMLInputElement
    const reader = new FileReader();
    reader.onload = (e: any) => {
      console.log(e);
      
      let xml = e.target.result;
      this.inputXml = xml;
      let result1 = converter.xml2json(xml, {compact: true, spaces: 2});

      const JSONData = JSON.parse(result1);
      const { Invoice } = JSONData
      // console.log("Invoice", Invoice);
      let prop = ""
      prop = "cbc:ID"
      const { [prop]: serieNumero } = Invoice
      // console.log("Serie y Numero", serieNumero);
      const valSerie = serieNumero._text.split("-")
      txtSerie.value = valSerie[0];
      txtNumero.value = valSerie[1];

      prop = "cac:TaxTotal"
      const { [prop]: monedaImporte } = Invoice
      // console.log("valores", monedaImporte);
      prop = "cbc:IssueDate"
      const { [prop]: fecha } = Invoice
      // console.log("fecha", fecha);
      txtFechaDoc.value = fecha._text;
		}
    reader.readAsText(event.target.files[0]);

    const fileCopy = event.target.files[0];
    this.extraerBase64(fileCopy).then((file: any) => {
      this.archivo1 = (file.base).split(',')[1];
      this.nomArchivo1 = fileCopy.name;
      this.msgError = "";
      this.errorValidacion = false;
    })
  }
}
