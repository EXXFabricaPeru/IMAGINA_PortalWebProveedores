import { Component } from '@angular/core';
import { ActivatedRoute, Router } from '@angular/router';
import { ProveedorService } from 'src/app/services/proveedor.service';
import { IconDirective } from '@coreui/icons-angular';
import { TextColorDirective, CardComponent, CardHeaderComponent, CardBodyComponent, AlertComponent, ContainerComponent, RowComponent, ColComponent, CardGroupComponent, 
  FormDirective, InputGroupComponent, InputGroupTextDirective, FormControlDirective, ButtonDirective,
  UtilitiesModule } from '@coreui/angular';
import { NgStyle, CommonModule } from '@angular/common';

@Component({
  selector: 'app-password-lost',
  standalone: true,
  imports: [AlertComponent, ContainerComponent, RowComponent, ColComponent, CardGroupComponent, TextColorDirective, CardComponent, CardHeaderComponent, CardBodyComponent, UtilitiesModule, FormDirective, 
    InputGroupComponent, InputGroupTextDirective, IconDirective, FormControlDirective, ButtonDirective, NgStyle, CommonModule],
  templateUrl: './password-lost.component.html',
  styleUrl: './password-lost.component.scss'
})
export class PasswordLostComponent {
  errorValidacion: Boolean = false;
  flagExito: Boolean = false;
  flagLoad: Boolean = false;
  msgError: string = "";
  codProveedor: string = "";
  
  constructor(private router: Router, private proveedorService: ProveedorService, private _route: ActivatedRoute) {
    let docEntry: string | null;
    docEntry = this._route.snapshot.paramMap.get("id") || "";
    this.codProveedor = docEntry;
  }

  async grabar(){
    this.errorValidacion = false;
    this.flagExito = false;
    const codigo: string = (document.getElementById("passOld") as HTMLInputElement).value;
    const passNew: string = (document.getElementById("passNew") as HTMLInputElement).value;
    const pasConf: string = (document.getElementById("passCon") as HTMLInputElement).value;

    if(passNew != pasConf){
      this.errorValidacion = true;
      this.msgError = "La nueva contraseña no coincide";
      return;
    }

    const data: any = {
      "code": this.codProveedor,
      "password": passNew,
      "ruc": codigo
    }

    this.flagLoad = true;
    await this.proveedorService.changePassword(data).toPromise().then(data => {
      this.msgError = data?.toString() || "";
      if(this.msgError != "Se actualizó con éxito la contraseña")
        this.errorValidacion = true;
      else
        this.flagExito = true;

      this.flagLoad = false;
    });
  }

  salir(){
    this.router.navigateByUrl('/login', { replaceUrl: true });
  }
} 
