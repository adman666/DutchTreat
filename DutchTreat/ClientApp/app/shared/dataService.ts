import { Injectable } from '@angular/core';
import { Http, Response, Headers } from '@angular/http';
import { Observable } from 'rxjs/Observable';
import { IProduct as Product } from './product';
import { Order, OrderItem } from './order';
import 'rxjs/add/operator/map';

@Injectable()
export class DataService {

    constructor(private readonly http: Http) {}

    private token: string = '';
    private tokenExpiration: Date;

    order = new Order();

    products: Product[] = [];

    loadProducts(): Observable<Product[]> {
        return this.http.get('/api/products')
            .map((result: Response) => this.products = result.json());
    }

    login(credentials) {
        return this.http.post('/account/createtoken', credentials)
            .map(response => {
                let tokenInfo = response.json();
                this.token = tokenInfo.token;
                this.tokenExpiration = tokenInfo.expiration;

                return true;
            });
    }

    checkout() {
        if (!this.order.orderNumber) {
            this.order.orderNumber = this.order.orderDate.getFullYear().toString() + this.order.orderDate.getTime().toString();
        }

        return this.http.post('/api/orders', this.order,
                {
                    headers: new Headers({
                        'Authorization': `Bearer ${this.token}`
                    })
                })
            .map(response => {

                this.order = new Order();

                return true;
            });
    }

    addToOrder(product: Product) {
        let item = this.order.items.find(x => x.productId === product.id);

        if (item) {
            item.quantity++;
        } else {
            item = new OrderItem();

            item.productId = product.id;
            item.productArtist = product.artist;
            item.productCategory = product.category;
            item.productArtId = product.artId;
            item.productTitle = product.title;
            item.productSize = product.size;
            item.unitPrice = product.price;

            item.quantity = 1;

            this.order.items.push(item);
        }
    }

    get loginRequired(): boolean {
        return this.token.length === 0 || this.tokenExpiration > new Date();
    }
}