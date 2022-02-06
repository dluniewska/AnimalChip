# AnimalChip
#### Serwis internetowy, w którym  w którym rejestrowane chipy zwierząt domowych.

## Spis treści 
* [Wprowadzenie](#wprowadzenie)
* [Specyfikacja wymagań](#specyfikacja-wymagań)
* [Stack Technologiczny](#stack-technologiczny)
* [Ilustracje](#ilustracje)

## Wprowadzenie 
 
Wiele ze zwierząt, które się zgubią, nigdy nie wraca do domu. W przypadku gdy doszło do kradzieży
zwierzęcia mogłoby być trudno udowodnić, że dane zwierzę jest naszym . Mimo, iż prężnie
funkcjonujące grupy w social mediach pomagają w odnalezieniu właściciela, nie zawsze udaje się
aby znaleziony zwierzak trafił z powrotem do swojego domu. Właściwa rejestracja sprawia, że można
temu zapobiec. Celem projektu było stworzenie serwisu internetowego, w którym rejestrowane są
zwierzęta domowe takie jak psy i koty. Niezalogowany użytkownik odwiedzjący stronę internetowa
może sprawdzić, czy podany chip znajduje się w bazie danych, natomiast zalogowany użytkownik,
może uzyskać informację na temat odnalezionego zwierzęcia, lub zarejestrować swoje zwierzęta.
Dodatkowa rola Administratora systemu umożliwia zarzadzanie wszystkim zwierzętami znajdującymi
się w bazie danych. 


## Specyfikacja wymagań
## Stack technologiczny
<ol>
<li> Architektura serwisu internetowego AnimalChip została przygotowana z wykorzystaniem
ASP .NET CORE 5 z zastosowaniem wzorca projektowego MVC.</li>
<li>Użytkownicy mają możliwość zalogowania się do serwisu z pomocą Facebooka, dzięki
wykorzystaniu otwartego standardu do autoryzacji OAuth 2.0. </li>
<li>Do dynamicznego wyszukiwania została zastosowana biblioteka JQuery. </li>
<li> Serwis stawia na rozbudowane bezpieczeństwo każdy formularz jest odpowiednio
walidowany cyberprzestępca nie ma możliwości przeprowadzić ataku XSS oraz SQL
Injection.</li>
<li> Strona jest udostępniona w czterech językach (polskim, angielskim, francuskim, hiszpańskim)
w modelu została wykorzystana wbudowana usługa globalizacji ASP .NET CORE.</li>
<li>Do ładowania bazy danych na stronie został zaimplementowany serwis AddMemoryCache()
z przekazaniem listy do zmiennej, aby skrócić czas odświeżania/ładowania strony. </li>
<li> Potwierdzenie konta użytkownika odbywa się poprzez skorzystanie przez niego z linka,
wysłanego na email, przy użyciu którego dokonał rejestracji</li>
<li>REST web api zostało zaimplementowane w postaci interfejsu Swagger oraz przy użyciu API
Kontrolera (testownego w Postmanie)</li>
<li>Własny Routing</li>
<li>Błędy zostały obsłużone blokami try/catch, natomiast formularze walidacją za pomocą
adnotacji.</li>
</ol>


## Ilustracje

![image](https://user-images.githubusercontent.com/35393983/152697301-7f1d10ec-be44-4d35-aa7e-2666ce169f24.png)
![2](https://user-images.githubusercontent.com/35393983/152697236-48c7ffcc-aecc-4020-8538-574a1af0fd19.JPG)
![3](https://user-images.githubusercontent.com/35393983/152697235-44e00b74-6292-46a3-a527-9087a93356f4.JPG)
![4](https://user-images.githubusercontent.com/35393983/152697232-ec2edc62-4dcf-476d-96cc-62ff4f379bde.JPG)
![5](https://user-images.githubusercontent.com/35393983/152697230-3a8a3e99-a08b-409b-97ae-66679760e127.JPG)
