import { AfterViewInit, Component } from '@angular/core';
import { CommonModule } from '@angular/common';
import { IconDirective } from '@coreui/icons-angular';
import { ContainerComponent, RowComponent, ColComponent, TextColorDirective, CardComponent, CardBodyComponent, FormDirective, InputGroupComponent, InputGroupTextDirective, 
        FormControlDirective, ButtonDirective, CardHeaderComponent, AlertComponent, FormCheckComponent, ModalModule} from '@coreui/angular';
import { DomSanitizer } from '@angular/platform-browser';
import { MaestroService } from 'src/app/services/maestro.service';
import { ProveedorService } from 'src/app/services/proveedor.service';
import { PdfJsViewerModule } from "ng2-pdfjs-viewer";
import { ActivatedRoute, Router } from '@angular/router';

@Component({
  selector: 'app-proveedor-mostrar',
  standalone: true,
  imports: [ContainerComponent, RowComponent, ColComponent, TextColorDirective, CardComponent, CardBodyComponent, FormDirective, InputGroupComponent, InputGroupTextDirective, IconDirective, FormControlDirective, ButtonDirective, CardHeaderComponent,
    CommonModule, AlertComponent, FormCheckComponent, ModalModule, PdfJsViewerModule
  ],
  templateUrl: './proveedor-mostrar.component.html',
  styleUrl: './proveedor-mostrar.component.scss'
})
export class ProveedorMostrarComponent implements AfterViewInit {
  tab1: boolean = true;
  tab2: boolean = false;
  tab3: boolean = false;
  tab4: boolean = false;
  tab5: boolean = false;
  flagLoad: boolean = false;
  confirmDialog: boolean = false;
  exitoDialog: boolean = false;

  listaContacto: any[] = [];
  listaCondicionPago: any;
  listaTipoCuenta: any;
  listaMoneda: any;
  listaDireccion: any[] = [];
  listaCuenta: any = [];
  listaBanco: any;
  listaDepartamento: any;
  listaProvincia: any;
  listaDistrito: any;
  
  provinciaSel: string = "";
  distritoSel: string = "";
  textButtonContacto: string = "Agregar";
  textButtonDireccion: string = "Agregar";
  textButtonCuenta: string = "Agregar";
  msgError: string = "";

  codProveedor: string = "";

  pdfSrc: string = "";
  archivo1: string = "assets/DeclaracionJuradaAntiCorrupcion_20304050601.pdf";
  nomArchivo1: string = "";
  archivo2: string = "";
  nomArchivo2: string = "";
  archivo3: string = "";
  nomArchivo3: string = "";
  archivo4: string = "";
  nomArchivo4: string = "";
  archivo5: string = "";
  nomArchivo5: string = "";
  archivo6: string = "";
  nomArchivo6: string = "";
  archivo7: string = "";
  nomArchivo7: string = "";
  archivo8: string = "";
  nomArchivo8: string = "";
  archivo9: string = "";
  nomArchivo9: string = "";
  archivo10: string = "";
  nomArchivo10: string = "";
  // archivo11: any;
  // nomArchivo11: string = "";
  // archivo12: any;
  // nomArchivo12: string = "";
  // archivo13: any;
  // nomArchivo13: string = "";
  // archivo14: any;
  // nomArchivo14: string = "";
  archivoaux1: any;
  nomArchivoaux1: string = "";
  flagArchivoaux1: boolean = false;
  archivoaux2: any;
  nomArchivoaux2: string = "";
  flagArchivoaux2: boolean = false;
  archivoaux3: any;
  nomArchivoaux3: string = "";
  flagArchivoaux3: boolean = false;
  archivoaux4: any;
  nomArchivoaux4: string = "";
  flagArchivoaux4: boolean = false;
  archivoaux5: any;
  nomArchivoaux5: string = "";
  flagArchivoaux5: boolean = false;

  proveedor: any = {estado: "Y"};
  
  constructor(private maestroService: MaestroService, private sanitizer: DomSanitizer, private proveedorService: ProveedorService, private _route: ActivatedRoute, private router: Router) { 
    const dataConfig: any = sessionStorage.getItem("config") || [];
    for(let i=0; i<dataConfig.length; i++){
      if(i==0){
        this.nomArchivoaux1 = dataConfig[i].descripcion;
        this.flagArchivoaux1 = dataConfig[i].flag01;
      }
      if(i==1){
        this.nomArchivoaux2 = dataConfig[i].descripcion;
        this.flagArchivoaux2 = dataConfig[i].flag01;
      }
      if(i==2){
        this.nomArchivoaux3 = dataConfig[i].descripcion;
        this.flagArchivoaux3 = dataConfig[i].flag01;
      }
      if(i==3){
        this.nomArchivoaux4 = dataConfig[i].descripcion;
        this.flagArchivoaux4 = dataConfig[i].flag01;
      }
      if(i==4){
        this.nomArchivoaux5 = dataConfig[i].descripcion;
        this.flagArchivoaux5 = dataConfig[i].flag01;
      }
    }

    maestroService.getCondicionesPago().toPromise().then(data => {
      this.listaCondicionPago = data;
      // console.log(this.listaCondicionPago);
    });
    
    maestroService.getMoneda().toPromise().then(data => {
      this.listaMoneda = data;
      // console.log(this.listaMoneda);
    });    
    
    maestroService.getDepartamento().toPromise().then(data => {
      this.listaDepartamento = data;
      // console.log(this.listaDepartamento);
    });
        
    maestroService.getBanco().toPromise().then(data => {
      this.listaBanco = data;
      // console.log(this.listaDepartamento);
    });    
        
    maestroService.getTipoCuenta().toPromise().then(data => {
      this.listaTipoCuenta = data;
      // console.log(this.listaDepartamento);
    });

    const docEntry: string = this._route.snapshot.paramMap.get("id") || "";
    // console.log("--->", docEntry);
    
    this.codProveedor = docEntry.substring(1);
    console.log(this.codProveedor);
  }
  
  ngAfterViewInit(){
    this.buscarProveedor(this.codProveedor);    
  }

  async buscarProveedor(code: string){
    const txtcodigo = document.getElementById("txtcodigo") as HTMLInputElement;
    const txtrazonsocial = document.getElementById("txtrazonsocial") as HTMLInputElement;
    const cmbCondPago = document.getElementById("cmbCondPago") as HTMLSelectElement;
    const cmbtipodoc = document.getElementById("cmbtipodoc") as HTMLSelectElement;
    const txtnrodoc = document.getElementById("txtnrodoc") as HTMLInputElement;
    const txtcorreo = document.getElementById("txtcorreo") as HTMLInputElement;
    const txttelfcli = document.getElementById("txttelfcli") as HTMLInputElement;
    const cbIsDetraccion = document.getElementById("cbIsDetraccion") as HTMLInputElement;

    await this.proveedorService.getProveedor(code).toPromise().then(data => {
      console.log("data", data);      
      this.proveedor = data;
      // console.log("data", this.proveedor);
      // console.log("codigo", proveedor.cardCode);      
      try {
        txtcodigo.value = this.proveedor.cardCode;
        txtrazonsocial.value = this.proveedor.cardName;
        cmbCondPago.value = this.proveedor.formaPago;
        cmbtipodoc.value = this.proveedor.u_EXX_TIPODOCU;
        txtnrodoc.value = this.proveedor.licTradNum;
        this.codProveedor = this.proveedor.licTradNum;
        txtcorreo.value = this.proveedor.emailAddress;
        txttelfcli.value = this.proveedor.phone1;
        // txtpassword.value = proveedor.password;
      } catch (error) {
        console.log(error);        
      }      

      this.listaContacto = this.proveedor.contactos;
      this.listaDireccion = this.proveedor.direcciones;
      this.listaCuenta = this.proveedor.cuentasBancarias;
    })
  }

  editarContacto(contacto: any){
    const _txtIdContacto = document.getElementById("txtCodContacto") as HTMLInputElement;
    const _txtPrimerNombre = document.getElementById("txtPrimerNombre") as HTMLInputElement;
    const _txtSegundNombre = document.getElementById("txtSegundoNombre") as HTMLInputElement;
    const _txtApellido = document.getElementById("txtApellidoCont") as HTMLInputElement;
    const _txtTelefono = document.getElementById("txtTelfContacto") as HTMLInputElement;
    const _txtCelular = document.getElementById("txtCelContacto") as HTMLInputElement;
    const _txtCargo = document.getElementById("txtCargoContac") as HTMLInputElement;
    const _txtCorreo = document.getElementById("txtEmailContacto") as HTMLInputElement;

    this.textButtonContacto = "Actualizar";
    console.log("contacto", contacto);

    _txtIdContacto.value = contacto.nombre;
    _txtTelefono.value = contacto.telefono;
    _txtCargo.value = contacto.cargo;
    _txtCorreo.value = contacto.email;
    _txtPrimerNombre.value = contacto.primerNombre;
    _txtSegundNombre.value = contacto.segundoNombre;
    _txtApellido.value = contacto.apellido;
    _txtCelular.value = contacto.celular;
  }

  async editarDireccion(dir: any){
    const _cmbDepartamento = document.getElementById("cmbdepartamento") as HTMLSelectElement;
    const _cmbProvincia = document.getElementById("cmbprovincia") as HTMLSelectElement;
    const _cmbDistrito = document.getElementById("cmbdistrito") as HTMLSelectElement;
    const _cmbTipoDir = document.getElementById("cmbtipodir") as HTMLSelectElement;
    const _txtDireccion = document.getElementById("txtdireccion") as HTMLInputElement;
    const _txtIdDir = document.getElementById("input-id-dir") as HTMLInputElement;

    console.log("direccion", dir);
    this.textButtonDireccion = "Actualizar";
    _txtIdDir.value = dir.address;
    _txtDireccion.value = dir.direccionDesc;
    _cmbTipoDir.value = dir.adressType
    _cmbDepartamento.value = dir.departamento;
    this.provinciaSel = dir.provincia;
    this.distritoSel = dir.distrito;

    await this.maestroService.getProvincia(dir.departamento).toPromise().then(data => {
      this.listaProvincia = data;
    });
    _cmbProvincia.value = dir.provincia;

    await this.maestroService.getDistrito(dir.provincia).toPromise().then(data => {
      this.listaDistrito = data;
    });
    _cmbDistrito.value = dir.distrito;
  }

  editarCuenta(cuenta: any){
    const txtnomcuenta = document.getElementById("txtnomcuenta") as HTMLInputElement;
    const cmbbanco = document.getElementById("cmbbanco") as HTMLSelectElement;
    const cmbmoneda = document.getElementById("cmbmoneda") as HTMLSelectElement;
    const cmbtipocuenta = document.getElementById("cmbtipocuenta") as HTMLSelectElement;
    const txtnrocuenta = document.getElementById("txtnrocuenta") as HTMLInputElement;
    const txtinterbancaria = document.getElementById("txtinterbancaria") as HTMLInputElement;
    const cbIsDetraccion = document.getElementById("cbIsDetraccion") as HTMLInputElement;
    
    this.textButtonCuenta = "Actualizar";
    console.log("cuenta", cuenta);

    txtnomcuenta.value = cuenta.acctName;
    cmbbanco.value = cuenta.bankCode;
    cmbmoneda.value = cuenta.moneda;
    cmbtipocuenta.value = cuenta.tipo;
    txtnrocuenta.value = cuenta.account;
    txtinterbancaria.value = cuenta.u_EXM_INTERBANCARIA;
    cbIsDetraccion.checked = cuenta.esDetraccion;
  }

  async downloadFile(code: string) {
    let nombre: string = "";
    if(code == "0"){
      for(let i = 0; i < 15; i++){
        switch ((i + 1).toString()){
          case "1":
            nombre = "DeclaracionJuradaAntiCorrupcion_" + this.codProveedor + ".pdf";
            break;
          case "2":
            nombre = "CodigoEticaConducta_" + this.codProveedor + ".pdf";
            break;
          case "3":
            nombre = "DeclaracionJuradaConfidencialidad_" + this.codProveedor + ".pdf";
            break;
          case "4":
            nombre = "DeclaracionJuradaCononocimiento_" + this.codProveedor + ".pdf";
            break;
          case "5":
            nombre = "FichaRUC_" + this.codProveedor + ".pdf";
            break;
          case "6":
            nombre = "ReporteTributarioSunat_" + this.codProveedor + ".pdf";
            break;
          case "7":
            nombre = "ReportePlataformaALFT_" + this.codProveedor + ".pdf";
            break;
          case "8":
            nombre = "ReporteCentralRiesgo_" + this.codProveedor + ".pdf";
            break;
          case "9":
            nombre = "ReporteR03_" + this.codProveedor + ".pdf";
            break;
          case "10":
            nombre = "RepresentanteLegalDni_" + this.codProveedor + ".pdf";
            break;
          case "11":
            nombre = this.nomArchivoaux1 + "_" + this.codProveedor + ".pdf";
            break;
          case "12":
            nombre = this.nomArchivoaux2 + "_" + this.codProveedor + ".pdf";
            break;
          case "13":
            nombre = this.nomArchivoaux3 + "_" + this.codProveedor + ".pdf";
            break;
          case "14":
            nombre = this.nomArchivoaux4 + "_" + this.codProveedor + ".pdf";
            break;
          case "15":
            nombre = this.nomArchivoaux5 + "_" + this.codProveedor + ".pdf";
            break;
        }

        console.log(nombre, (i + 1).toString());
        
        await this.proveedorService.downloadFile((i + 1).toString(), this.codProveedor).toPromise().then(data => {
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
    }else{
      switch (code){
        case "1":
          nombre = "DeclaracionJuradaAntiCorrupcion_" + this.codProveedor + ".pdf";
            break;
        case "2":
          nombre = "CodigoEticaConducta_" + this.codProveedor + ".pdf";
            break;
        case "3":
          nombre = "DeclaracionJuradaConfidecialidad_" + this.codProveedor + ".pdf";
            break;
        case "4":
          nombre = "DeclaracionJuradaCononocimiento_" + this.codProveedor + ".pdf";
            break;
        case "5":
          nombre = "FichaRUC_" + this.codProveedor + ".pdf";
            break;
        case "6":
          nombre = "ReporteTributarioSunat_" + this.codProveedor + ".pdf";
            break;
        case "7":
          nombre = "ReportePlataformaALFT_" + this.codProveedor + ".pdf";
            break;
        case "8":
          nombre = "ReporteCentralRiesgo_" + this.codProveedor + ".pdf";
            break;
        case "9":
          nombre = "ReporteR03_" + this.codProveedor + ".pdf";
            break;
        case "10":
          nombre = "RepresentanteLegalDni_" + this.codProveedor + ".pdf";
            break;
      }

      this.proveedorService.downloadFile(code, this.codProveedor).toPromise().then(data => {
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
  }

  async aprobar(){
    const cmbdecision = document.getElementById("cmbdecision") as HTMLSelectElement;
    const txtcomentario = document.getElementById("txtComentario") as HTMLInputElement;
    this.proveedor.decision = cmbdecision.value;
    this.proveedor.comentario = txtcomentario.value;
    this.flagLoad = true;
    await this.proveedorService.aprobarProveedor(this.proveedor).toPromise().then(data => {
      console.log(data);
      
      const xRpta: string = data || "";
      console.log(xRpta);
       this.msgError = xRpta;
      this.flagLoad = false;

      if(this.msgError.includes("éxito"))
        this.exitoDialog = true;
      else
        this.confirmDialog = true;
    });

  }

  btnOk(){
    this.exitoDialog = false;
    this.router.navigateByUrl('/proveedor-listar', { replaceUrl: true });
  }
}
