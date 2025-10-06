 
Custom app developed to help a relative at their job. Consists of three tabs that work in sequence to speed up the process of sharing payment info, generating files ready to be sent, and a fourth tab that allows a small degree of customization.

The first tab, named "Datos Clientes" (Custormers' Data), uses a spreadsheet to store data from individual customer in a way similar to that of an actual database. (Note: this was mostly a way to test spreadsheet management in a windows app. DATA IS EXPOSED AND NOT PROTECTED).
The user can easily add new and edit/delete old customers using simple input dialogs. Each entry consists of three fields: name, ID and IBAN. Fields cannot be left empty, but there's no other restrictions, meaning that missing info can just be added as "-" or another similar character. This tab also includes a serach bar that allows searching customers using their name.

<img width="786" height="443" alt="image" src="https://github.com/user-attachments/assets/8ee13293-9b99-426f-8bde-84476f2abd44" />

<img width="401" height="175" alt="image" src="https://github.com/user-attachments/assets/bd63890e-4092-4440-82b5-4278cc21cf73" />  

<br/><br/>
Once the "database" is filled with custormers' data, the second tab, "Pagos" (Payments), can be used to create individual payments. This payments are volatile, meaning that once the app is closed they are deleted/not stored; and can be added, edited and removed just like the customers' data. Each payment is assigned to a customer that MUST be part of the previous "database". Along with the customer's name, a payment requires the amount of work hours (in the "Horas totales" field) and possible additional fees (the "Extras" field). Both of these fields must be numbers, but can be 0. (They can also be negative numbers for weird edge cases, but that is not common). Lastly, an optional comment can be added if needed. This last field can be left empty.


<img width="786" height="443" alt="image" src="https://github.com/user-attachments/assets/792b4c49-2b9d-4e9b-b601-3422707cb9d8" />


<img width="422" height="279" alt="image" src="https://github.com/user-attachments/assets/96975a76-4300-45a1-a68a-6063edc8aa95" />

<br/><br/>
The third and final tab in the process of sharing the info is the shortest and simplest. It only has two buttons that each creates one of the files. The first button, labeled "Generar archivos de Pagos" (Create payments file) generates a .xlsx file that contains a breakdown of each payment. The second button, labeled "Generar archivos de Datos y Horas" (Create data and workhours file) generates a .doc file that lists each of the payments along with all the information related to both the payment and the customer, essentially combining the info of both the "Datos Clientes" and the "Pagos" tabs. This tab also shows the directories in which each file will be saved.


<img width="786" height="443" alt="image" src="https://github.com/user-attachments/assets/5b82912b-43ba-45cb-b6c1-735d0be02713" />

Payment file.

<img width="583" height="225" alt="image" src="https://github.com/user-attachments/assets/a7dff190-4776-45c9-a19e-dbd71ca5617a" />

Data and workhours file

<img width="340" height="480" alt="image" src="https://github.com/user-attachments/assets/a86015a6-a19a-4be1-8a1f-dc29ea3bb26f" />


<br/><br/>
The last tab in the app is the "Opciones" (Options) tab. It includes two groups of options that can be modified. The first is payment related options, allowing to change the value of the "IVA" (VAT tax) and the "Precio por hora" (Price per hour). The second group is file related options, allowing to change the default directory to save both files independently. Files are not saved instantly when pressing their respective button, so this option serves to determine the starting point for the saving dialogs that open when creating the files.

<img width="786" height="443" alt="image" src="https://github.com/user-attachments/assets/f4293837-1d87-4ecb-baa4-8832f8e5950b" />


---

Libraries used in this proyect:

[WPF](https://learn.microsoft.com/es-es/dotnet/desktop/wpf/overview/)

[Spire.XLS](https://www.e-iceblue.com/Tutorials/Spire.XLS/Spire.XLS-Program-Guide/Spire.XLS-Program-Guide-Content.html)

[Spire.Doc](https://www.e-iceblue.com/Tutorials/Spire.Doc/Spire.Doc-Program-Guide/Spire.Doc-Program-Guide-Content.html)

Both of Spire's libraries where used in their free version.
