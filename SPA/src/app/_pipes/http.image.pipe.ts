import { Pipe, PipeTransform } from '@angular/core';

@Pipe({
  name: 'httpImage'
})
export class HttpImagePipe implements PipeTransform {

  transform(value: unknown, ...args: unknown[]): unknown {
    return null;
  }

}
