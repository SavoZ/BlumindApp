import { Component, OnInit, Inject } from '@angular/core';
import { HttpClient, HttpParams } from '@angular/common/http';
import { Router, ActivatedRoute } from '@angular/router';
import { ProductEditModel } from 'src/app/_models/productEdit';
import { API_ENDPOINT } from 'src/app/_constants/url.constants';
import { QueryParamsSearch } from 'src/app/_helper.ts/query_params_search';

@Component({
  selector: 'app-edit-product',
  templateUrl: './edit-product.component.html',
  styleUrls: ['./edit-product.component.css']
})
export class EditProductComponent implements OnInit {

  constructor(private http: HttpClient, private router: Router, private route: ActivatedRoute) {
  }

  model = new ProductEditModel();
  ngOnInit() {
    const id = QueryParamsSearch.findParam(this.route, 'id');
    if (id) {
      this.http.get<ProductEditModel>(API_ENDPOINT + 'Product/GetProduct', { params: new HttpParams().set('productId', id) }).toPromise()
        .then(data => {
          this.model = data;
        });
    }
  }

  public submit() {
    console.log(this.model);
    this.http.post<ProductEditModel>(API_ENDPOINT + 'Product/PostProduct', this.model)
      .toPromise().then(() => {
        // this.router.navigate(['countries']);
      });
  }

}
