import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { ProjectSuggestionsResultComponent } from './project-suggestions-result.component';

describe('ProjectSuggestionsResultComponent', () => {
  let component: ProjectSuggestionsResultComponent;
  let fixture: ComponentFixture<ProjectSuggestionsResultComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ ProjectSuggestionsResultComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(ProjectSuggestionsResultComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
