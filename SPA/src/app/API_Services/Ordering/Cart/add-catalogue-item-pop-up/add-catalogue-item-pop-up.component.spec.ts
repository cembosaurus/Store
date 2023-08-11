import { ComponentFixture, TestBed } from '@angular/core/testing';

import { AddCatalogueItemPopUpComponent } from './add-catalogue-item-pop-up.component';

describe('AddCatalogueItemPopUpComponent', () => {
  let component: AddCatalogueItemPopUpComponent;
  let fixture: ComponentFixture<AddCatalogueItemPopUpComponent>;

  beforeEach(() => {
    TestBed.configureTestingModule({
      declarations: [AddCatalogueItemPopUpComponent]
    });
    fixture = TestBed.createComponent(AddCatalogueItemPopUpComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
