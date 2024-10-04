import { Pipe, PipeTransform } from '@angular/core';

@Pipe({
  name: 'pagProveedor',
  standalone: true
})
export class PagProveedorPipe implements PipeTransform {

  transform(documentos: any[], page: number): any[] {
    let _page: number;
    if(page != 0)
      _page = (10 * (page - 1));
    else
      _page = page;

    // console.log("pagina", _page);
    return documentos.slice(_page, _page + 10);
  }

}
