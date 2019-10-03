import { Component, OnInit, Inject } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Router } from '@angular/router';
import { ProductEdit } from 'src/app/_models/productEdit';
import { API_ENDPOINT } from 'src/app/_constants/url.constants';

@Component({
  selector: 'app-edit-product',
  templateUrl: './edit-product.component.html',
  styleUrls: ['./edit-product.component.css']
})
export class EditProductComponent implements OnInit {

  constructor(private http: HttpClient, private router: Router) {
  }

  model = new ProductEdit();
  ngOnInit() {
  }

  public submit() {
    console.log(this.model);
    this.http.post<ProductEdit>(API_ENDPOINT + 'Product/PostProduct', this.model)
      .toPromise().then(() => {
        // this.router.navigate(['countries']);
      });
  }

  // public submit() {
  //   console.log(this.model);
  //   this.http.get<ProductEdit>(this.baseUrl + 'Products/GetProduct')
  //     .toPromise().then(() => {
  //       // this.router.navigate(['countries']);
  //     });
  // }

}
