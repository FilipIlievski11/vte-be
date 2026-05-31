namespace VTE.WPF.Resources;

public static class Strings
{
    private static bool _isMacedonian = true;
    public static bool IsMacedonian => _isMacedonian;

    public static void SetLanguage(bool macedonian)
    {
        _isMacedonian = macedonian;
    }

    // App
    public static string AppTitle => _isMacedonian ? "ВТЕ - Систем за технички преглед на возила" : "VTE - Vehicle Technical Examination System";
    public static string Welcome => _isMacedonian ? "Добредојдовте" : "Welcome";
    public static string Login => _isMacedonian ? "Најави се" : "Login";
    public static string Username => _isMacedonian ? "Корисничко име" : "Username";
    public static string Password => _isMacedonian ? "Лозинка" : "Password";
    public static string InvalidCredentials => _isMacedonian ? "Невалидно корисничко име или лозинка." : "Invalid username or password.";
    public static string ConnectionError => _isMacedonian ? "Грешка при конекција:" : "Connection error:";
    public static string PleaseEnterCredentials => _isMacedonian ? "Внесете корисничко име и лозинка." : "Please enter username and password.";

    // Navigation
    public static string Dashboard => _isMacedonian ? "Контролна табла" : "Dashboard";
    public static string Customers => _isMacedonian ? "Клиенти" : "Customers";
    public static string Vehicles => _isMacedonian ? "Возила" : "Vehicles";
    public static string Requests => _isMacedonian ? "Барања" : "Requests";
    public static string TechnicalExams => _isMacedonian ? "Технички прегледи" : "Technical Exams";
    public static string Documents => _isMacedonian ? "Документи" : "Documents";
    public static string Payments => _isMacedonian ? "Плаќања" : "Payments";
    public static string Reports => _isMacedonian ? "Извештаи" : "Reports";
    public static string Lookups => _isMacedonian ? "Шифрарници" : "Lookups";
    public static string Administration => _isMacedonian ? "Администрација" : "Administration";
    public static string Operations => _isMacedonian ? "ОПЕРАЦИИ" : "OPERATIONS";
    public static string Analytics => _isMacedonian ? "АНАЛИТИКА" : "ANALYTICS";
    public static string Configuration => _isMacedonian ? "КОНФИГУРАЦИЈА" : "CONFIGURATION";

    // Common actions
    public static string Save => _isMacedonian ? "Зачувај" : "Save";
    public static string Cancel => _isMacedonian ? "Откажи" : "Cancel";
    public static string Delete => _isMacedonian ? "Избриши" : "Delete";
    public static string Search => _isMacedonian ? "Пребарај" : "Search";
    public static string NewRecord => _isMacedonian ? "Нов запис" : "New Record";
    public static string BackToList => _isMacedonian ? "< Назад кон листа" : "< Back to List";
    public static string Edit => _isMacedonian ? "Измени" : "Edit";
    public static string Close => _isMacedonian ? "Затвори" : "Close";
    public static string Yes => _isMacedonian ? "Да" : "Yes";
    public static string No => _isMacedonian ? "Не" : "No";
    public static string Ready => _isMacedonian ? "Подготвено" : "Ready";
    public static string Loading => _isMacedonian ? "Се вчитува..." : "Loading...";
    public static string Saving => _isMacedonian ? "Зачувување..." : "Saving...";
    public static string Records => _isMacedonian ? "записи" : "records";
    public static string Page => _isMacedonian ? "Страна" : "Page";
    public static string Of => _isMacedonian ? "од" : "of";
    public static string Prev => _isMacedonian ? "< Претходна" : "< Prev";
    public static string Next => _isMacedonian ? "Следна >" : "Next >";

    // Customer fields
    public static string NewCustomer => _isMacedonian ? "+ Нов клиент" : "+ New Customer";
    public static string FirstName => _isMacedonian ? "Име" : "First Name";
    public static string LastName => _isMacedonian ? "Презиме" : "Last Name";
    public static string ParentName => _isMacedonian ? "Татково име" : "Parent Name";
    public static string DateOfBirth => _isMacedonian ? "Датум на раѓање" : "Date of Birth";
    public static string IdentificationNumber => _isMacedonian ? "Матичен број" : "Identification Number";
    public static string IdentityCardNumber => _isMacedonian ? "БЛК" : "Identity Card Number";
    public static string DrivingLicenseNumber => _isMacedonian ? "Возачка дозвола" : "Driving License Number";
    public static string IsCompany => _isMacedonian ? "Правно лице" : "Company";
    public static string CompanyName => _isMacedonian ? "Назив на фирма" : "Company Name";
    public static string TaxNumber => _isMacedonian ? "Даночен број" : "Tax Number";
    public static string PhoneNumber => _isMacedonian ? "Телефон" : "Phone";
    public static string Email => _isMacedonian ? "Е-пошта" : "Email";
    public static string Fax => _isMacedonian ? "Факс" : "Fax";
    public static string Occupation => _isMacedonian ? "Занимање" : "Occupation";
    public static string Note => _isMacedonian ? "Забелешка" : "Note";
    public static string City => _isMacedonian ? "Град" : "City";

    // Vehicle fields
    public static string NewVehicle => _isMacedonian ? "+ Ново возило" : "+ New Vehicle";
    public static string ShellNumber => _isMacedonian ? "Број на шасија" : "Shell Number";
    public static string EngineNumber => _isMacedonian ? "Број на мотор" : "Engine Number";
    public static string RegistrationNumber => _isMacedonian ? "Регистарски број" : "Registration Number";
    public static string VehicleModel => _isMacedonian ? "Комерцијална ознака" : "Vehicle Model";
    public static string VehicleCategory => _isMacedonian ? "Категорија" : "Category";
    public static string BodyType => _isMacedonian ? "Форма на каросерија" : "Body Type";
    public static string EngineType => _isMacedonian ? "Тип на мотор" : "Engine Type";
    public static string EnginePower => _isMacedonian ? "Моќност (kW)" : "Engine Power (kW)";
    public static string MakeDate => _isMacedonian ? "Датум на производство" : "Make Date";
    public static string Registration => _isMacedonian ? "Регистрација" : "Registration";
    public static string Classification => _isMacedonian ? "Класификација" : "Classification";
    public static string Engine => _isMacedonian ? "Мотор" : "Engine";
    public static string Dimensions => _isMacedonian ? "Димензии" : "Dimensions";
    public static string Seating => _isMacedonian ? "Седишта" : "Seating";
    public static string Other => _isMacedonian ? "Останато" : "Other";
    public static string Owner => _isMacedonian ? "Сопственик" : "Owner";
    public static string Year => _isMacedonian ? "Година" : "Year";

    // Technical Exams
    public static string NewExam => _isMacedonian ? "+ Нов преглед" : "+ New Exam";
    public static string ExamDate => _isMacedonian ? "Датум на преглед" : "Exam Date";
    public static string ValidUntil => _isMacedonian ? "Важи до" : "Valid Until";
    public static string Passed => _isMacedonian ? "Исправно" : "Passed";
    public static string Failed => _isMacedonian ? "Неисправно" : "Failed";
    public static string VehiclePassed => _isMacedonian ? "Возилото е исправно" : "Vehicle Passed";
    public static string BrakeMeasurements => _isMacedonian ? "Мерења на сопирачки" : "Brake Measurements";
    public static string BrakeEffectiveness => _isMacedonian ? "Ефективност на сопирачки" : "Brake Effectiveness";
    public static string Emissions => _isMacedonian ? "Емисии" : "Emissions";
    public static string Notes => _isMacedonian ? "Забелешки" : "Notes";
    public static string Controller => _isMacedonian ? "Контролор" : "Controller";
    public static string Organization => _isMacedonian ? "Организација" : "Organization";
    public static string ExamType => _isMacedonian ? "Тип на преглед" : "Exam Type";

    // Payments
    public static string NewPayment => _isMacedonian ? "+ Ново плаќање" : "+ New Payment";
    public static string DocumentNumber => _isMacedonian ? "Број на документ" : "Document Number";
    public static string PaymentDate => _isMacedonian ? "Датум на плаќање" : "Payment Date";
    public static string DueDate => _isMacedonian ? "Рок на плаќање" : "Due Date";
    public static string Discount => _isMacedonian ? "Попуст (%)" : "Discount (%)";
    public static string IsPaid => _isMacedonian ? "Платено" : "Paid";
    public static string IsCancelled => _isMacedonian ? "Сторнирано" : "Cancelled";
    public static string PaymentType => _isMacedonian ? "Тип на плаќање" : "Payment Type";
    public static string LineItems => _isMacedonian ? "Ставки" : "Line Items";
    public static string Description => _isMacedonian ? "Опис" : "Description";
    public static string Quantity => _isMacedonian ? "Количина" : "Quantity";
    public static string UnitPrice => _isMacedonian ? "Цена" : "Unit Price";
    public static string VATPercent => _isMacedonian ? "ДДВ (%)" : "VAT (%)";

    // Requests
    public static string NewRequest => _isMacedonian ? "+ Ново барање" : "+ New Request";
    public static string RequestType => _isMacedonian ? "Тип на барање" : "Request Type";
    public static string DateCreated => _isMacedonian ? "Датум на креирање" : "Date Created";
    public static string DateEnded => _isMacedonian ? "Датум на завршување" : "Date Ended";
    public static string Status => _isMacedonian ? "Статус" : "Status";
    public static string Open => _isMacedonian ? "Отворено" : "Open";
    public static string Closed => _isMacedonian ? "Затворено" : "Closed";

    // Documents
    public static string NewDocument => _isMacedonian ? "+ Нов документ" : "+ New Document";
    public static string DocumentType => _isMacedonian ? "Тип на документ" : "Document Type";

    // Dashboard
    public static string GoodMorning => _isMacedonian ? "Добро утро" : "Good morning";
    public static string GoodAfternoon => _isMacedonian ? "Добар ден" : "Good afternoon";
    public static string GoodEvening => _isMacedonian ? "Добра вечер" : "Good evening";
    public static string Registered => _isMacedonian ? "регистрирани" : "registered";
    public static string InRegistry => _isMacedonian ? "во регистар" : "in registry";
    public static string Inspections => _isMacedonian ? "прегледи" : "inspections";
    public static string PendingPayments => _isMacedonian ? "неплатени" : "pending payments";
    public static string QuickActions => _isMacedonian ? "Брзи акции" : "Quick Actions";
    public static string RecentActivity => _isMacedonian ? "Последна активност" : "Recent Activity";
    public static string OpenRequests => _isMacedonian ? "Отворени барања" : "Open Requests";
    public static string DocumentsIssued => _isMacedonian ? "Издадени документи" : "Documents Issued";
    public static string ExamsThisMonth => _isMacedonian ? "Прегледи овој месец" : "Exams This Month";
    public static string NoRecentActivity => _isMacedonian ? "Нема последна активност" : "No recent activity yet";
    public static string CustomerRegistered => _isMacedonian ? "Регистриран клиент:" : "Customer registered:";
    public static string ExamResult => _isMacedonian ? "Преглед" : "Exam";
    public static string PaymentStatus => _isMacedonian ? "Плаќање" : "Payment";
    public static string RegisterVehicle => _isMacedonian ? "Регистрирај возило" : "Register Vehicle";
    public static string NewTechnicalExam => _isMacedonian ? "Нов технички преглед" : "New Technical Exam";
    public static string ExamsToday => _isMacedonian ? "ПРЕГЛЕДИ ДЕНЕС" : "EXAMS TODAY";
    public static string Unpaid => _isMacedonian ? "НЕПЛАТЕНИ" : "UNPAID";

    // Admin
    public static string Users => _isMacedonian ? "Корисници" : "Users";
    public static string NewUser => _isMacedonian ? "+ Нов корисник" : "+ New User";
    public static string FullName => _isMacedonian ? "Целосно име" : "Full Name";
    public static string Role => _isMacedonian ? "Улога" : "Role";
    public static string IsActive => _isMacedonian ? "Активен" : "Active";

    // Reports
    public static string CustomerSummary => _isMacedonian ? "Преглед на клиенти" : "Customer Summary";
    public static string VehicleRegistry => _isMacedonian ? "Регистар на возила" : "Vehicle Registry";
    public static string TechnicalExamsReport => _isMacedonian ? "Извештај за прегледи" : "Technical Exams Report";
    public static string PaymentSummary => _isMacedonian ? "Преглед на плаќања" : "Payment Summary";
    public static string UnpaidPayments => _isMacedonian ? "Неплатени плаќања" : "Unpaid Payments";
    public static string ExamsByOrganization => _isMacedonian ? "Прегледи по организација" : "Exams by Organization";
    public static string MonthlyActivity => _isMacedonian ? "Месечна активност" : "Monthly Activity";
    public static string Generate => _isMacedonian ? "Генерирај" : "Generate";
    public static string From => _isMacedonian ? "Од" : "From";
    public static string To => _isMacedonian ? "До" : "To";
    public static string Total => _isMacedonian ? "Вкупно" : "Total";
    public static string PassedCount => _isMacedonian ? "Исправни" : "Passed";
    public static string FailedCount => _isMacedonian ? "Неисправни" : "Failed";
    public static string PaidCount => _isMacedonian ? "Платени" : "Paid";
    public static string UnpaidCount => _isMacedonian ? "Неплатени" : "Unpaid";
    public static string Companies => _isMacedonian ? "Правни лица" : "Companies";
    public static string Individuals => _isMacedonian ? "Физички лица" : "Individuals";
    public static string WithEmail => _isMacedonian ? "Со е-пошта" : "With Email";
    public static string WithPhone => _isMacedonian ? "Со телефон" : "With Phone";
    public static string PassRate => _isMacedonian ? "Процент поминати" : "Pass Rate";
    public static string Month => _isMacedonian ? "Месец" : "Month";
    public static string NewCustomersCount => _isMacedonian ? "Нови клиенти" : "New Customers";
    public static string NewVehiclesCount => _isMacedonian ? "Нови возила" : "New Vehicles";
    public static string ExamsCount => _isMacedonian ? "Прегледи" : "Exams";
    public static string PaymentsCount => _isMacedonian ? "Плаќања" : "Payments";

    // Detail page titles
    public static string General => _isMacedonian ? "Општо" : "General";
    public static string NewExamTitle => _isMacedonian ? "Нов преглед" : "New Exam";
    public static string EditExamTitle => _isMacedonian ? "Измени преглед" : "Edit Exam";
    public static string NewCustomerTitle => _isMacedonian ? "Нов клиент" : "New Customer";
    public static string EditCustomerTitle => _isMacedonian ? "Измени клиент" : "Edit Customer";
    public static string NewVehicleTitle => _isMacedonian ? "Ново возило" : "New Vehicle";
    public static string EditVehicleTitle => _isMacedonian ? "Измени возило" : "Edit Vehicle";
    public static string NewPaymentTitle => _isMacedonian ? "Ново плаќање" : "New Payment";
    public static string EditPaymentTitle => _isMacedonian ? "Измени плаќање" : "Edit Payment";
    public static string NewRequestTitle => _isMacedonian ? "Ново барање" : "New Request";
    public static string EditRequestTitle => _isMacedonian ? "Измени барање" : "Edit Request";
    public static string NewDocumentTitle => _isMacedonian ? "Нов документ" : "New Document";
    public static string EditDocumentTitle => _isMacedonian ? "Измени документ" : "Edit Document";

    // Exam detail fields
    public static string RegistrationNumberRequired => _isMacedonian ? "Регистарски број *" : "Registration Number *";
    public static string ExamDateRequired => _isMacedonian ? "Датум на преглед *" : "Exam Date *";
    public static string ValidUntilDateRequired => _isMacedonian ? "Важи до *" : "Valid Until Date *";
    public static string ExamTypeRequired => _isMacedonian ? "Тип на преглед *" : "Exam Type *";
    public static string OrganizationRequired => _isMacedonian ? "Организација *" : "Organization *";
    public static string FirstControllerRequired => _isMacedonian ? "Прв контролор *" : "First Controller *";
    public static string CustomerVehicleRelation => _isMacedonian ? "Клиент-Возило" : "Customer-Vehicle Relation";
    public static string CustomerVehicleRelationRequired => _isMacedonian ? "Клиент-Возило *" : "Customer-Vehicle Relation *";
    public static string BrakeMeasurementsAxle1 => _isMacedonian ? "Мерења на сопирачки - Оска 1" : "Brake Measurements - Axle 1";
    public static string BrakeMeasurementsAxle2 => _isMacedonian ? "Мерења на сопирачки - Оска 2" : "Brake Measurements - Axle 2";
    public static string WorkingBrakeEmpty => _isMacedonian ? "Работна сопир. празно" : "Working Brake Effectiveness (Empty)";
    public static string WorkingBrakeFull => _isMacedonian ? "Работна сопир. полно" : "Working Brake Effectiveness (Full)";
    public static string SecondaryBrake => _isMacedonian ? "Помошна сопирачка" : "Secondary Brake Effectiveness";
    public static string ParkingBrake => _isMacedonian ? "Паркинг сопирачка" : "Parking Brake Effectiveness";
    public static string VehicleWeight => _isMacedonian ? "Маса на возило (KG)" : "Vehicle Weight (KG)";
    public static string EngineSpeedRPM => _isMacedonian ? "Брзина на вртење (RPM)" : "Engine Speed (RPM)";
    public static string EngineTurns => _isMacedonian ? "Вртежи на мотор" : "Engine Turns";
    public static string COPlusTurns => _isMacedonian ? "CO + Вртежи" : "CO + Turns";
    public static string Lambda => _isMacedonian ? "Ламбда" : "Lambda";
    public static string Pinpoints => _isMacedonian ? "Точки" : "Pinpoints";
    public static string NoiseDB => _isMacedonian ? "Бучава (dB)" : "Noise (dB)";
    public static string EngineOilTemperature => _isMacedonian ? "Температура на масло (C)" : "Engine Oil Temperature (C)";
    public static string TechnicalChanges => _isMacedonian ? "Технички промени" : "Technical Changes";
    public static string ExplanationNote => _isMacedonian ? "Образложение" : "Explanation Note";
    public static string DriverWarning => _isMacedonian ? "Предупредување за возач" : "Driver Warning";

    // Customer detail fields
    public static string FirstNameRequired => _isMacedonian ? "Име *" : "First Name *";
    public static string LastNameRequired => _isMacedonian ? "Презиме *" : "Last Name *";
    public static string IdentityCardNumberBLK => _isMacedonian ? "БЛК" : "Identity Card Number (BLK)";
    public static string IsCompanyLabel => _isMacedonian ? "Правно лице" : "Is Company";

    // Vehicle detail fields
    public static string ShellNumberRequired => _isMacedonian ? "Број на шасија *" : "Shell Number *";
    public static string FirstRegistrationNumber => _isMacedonian ? "Прв регистарски број" : "First Registration Number";
    public static string FirstRegistrationDate => _isMacedonian ? "Прва регистрација" : "First Registration Date";
    public static string LastRegistrationNumber => _isMacedonian ? "Последен регистарски број" : "Last Registration Number";
    public static string LastRegistrationDate => _isMacedonian ? "Последна регистрација" : "Last Registration Date";
    public static string PaymentCategoryField => _isMacedonian ? "Категорија за плаќање" : "Payment Category";
    public static string UseType => _isMacedonian ? "Намена" : "Use Type";
    public static string EnginePowerKW => _isMacedonian ? "Моќност (KW)" : "Engine Power (KW)";
    public static string EngineTorqueNM => _isMacedonian ? "Вртежен момент (NM)" : "Engine Torque (NM)";
    public static string EngineCapacity => _isMacedonian ? "Зафатнина (CM3)" : "Engine Working Capacity (CM3)";
    public static string HeightMM => _isMacedonian ? "Висина (MM)" : "Height (MM)";
    public static string WidthMM => _isMacedonian ? "Ширина (MM)" : "Width (MM)";
    public static string LengthMM => _isMacedonian ? "Должина (MM)" : "Length (MM)";
    public static string EmptyWeightKG => _isMacedonian ? "Празна маса (KG)" : "Empty Weight (KG)";
    public static string MaxWeightKG => _isMacedonian ? "Макс. дозволена маса (KG)" : "Max Allowed Weight (KG)";
    public static string NumberOfDoors => _isMacedonian ? "Број на врати" : "Number of Doors";
    public static string NumberOfSeats => _isMacedonian ? "Број на седишта" : "Number of Seats";
    public static string MaxSpeedKMH => _isMacedonian ? "Макс. брзина (KM/H)" : "Max Speed (KM/H)";
    public static string HasLPG => _isMacedonian ? "ТНГ" : "Has LPG";
    public static string HasHook => _isMacedonian ? "Кука" : "Has Hook";

    // Payment detail fields
    public static string DocumentNumberRequired => _isMacedonian ? "Број на документ *" : "Document Number *";
    public static string DiscountPercent => _isMacedonian ? "Попуст (%)" : "Discount Percent";
    public static string PaymentTypeRequired => _isMacedonian ? "Тип на плаќање *" : "Payment Type *";
    public static string IsPaidLabel => _isMacedonian ? "Платено" : "Is Paid";
    public static string IsCancelledLabel => _isMacedonian ? "Сторнирано" : "Is Cancelled";

    // Request detail fields
    public static string RequestTypeRequired => _isMacedonian ? "Тип на барање *" : "Request Type *";
    public static string CustomerVehicleRequired => _isMacedonian ? "Клиент - Возило *" : "Customer - Vehicle *";
    public static string IsCustomerChanged => _isMacedonian ? "Промена на клиент" : "Is Customer Changed";
    public static string IsVehicleChanged => _isMacedonian ? "Промена на возило" : "Is Vehicle Changed";

    // Document detail fields
    public static string DocumentTypeRequired => _isMacedonian ? "Тип на документ *" : "Document Type *";

    // Request Wizard
    public static string Step => _isMacedonian ? "Чекор" : "Step";
    public static string Of2 => _isMacedonian ? "од" : "of";
    public static string SelectRequestType => _isMacedonian ? "Избери тип на барање" : "Select Request Type";
    public static string SelectCustomerVehicle => _isMacedonian ? "Избери клиент и возило" : "Select Customer & Vehicle";
    public static string DocumentChecklist => _isMacedonian ? "Проверка на документи" : "Document Checklist";
    public static string TechnicalExamCheck => _isMacedonian ? "Технички преглед" : "Technical Exam";
    public static string SummaryAndComplete => _isMacedonian ? "Преглед и завршување" : "Summary & Complete";
    public static string NextStep => _isMacedonian ? "Следно >" : "Next >";
    public static string PreviousStep => _isMacedonian ? "< Претходно" : "< Previous";
    public static string CompleteRequest => _isMacedonian ? "Заврши барање" : "Complete Request";
    public static string TechExamRequired => _isMacedonian ? "Потребен технички преглед" : "Technical Exam Required";
    public static string PaymentRequired => _isMacedonian ? "Потребно плаќање" : "Payment Required";
    public static string NewRegistration => _isMacedonian ? "Нова регистрација" : "New Registration";
    public static string OwnershipProof => _isMacedonian ? "Доказ за сопственост" : "Ownership Proof";
    public static string PaymentProof => _isMacedonian ? "Доказ за плаќање" : "Payment Proof";
    public static string SearchCustomer => _isMacedonian ? "Пребарај клиент" : "Search Customer";
    public static string SearchVehicle => _isMacedonian ? "Пребарај возило" : "Search Vehicle";
    public static string NoTechExam => _isMacedonian ? "Нема технички преглед за ова возило" : "No technical exam found for this vehicle";
    public static string RequestCompleted => _isMacedonian ? "Барањето е успешно завршено!" : "Request completed successfully!";
    public static string YesText => _isMacedonian ? "Да" : "Yes";
    public static string NoText => _isMacedonian ? "Не" : "No";
    public static string Customer => _isMacedonian ? "Клиент" : "Customer";
    public static string VehicleLabel => _isMacedonian ? "Возило" : "Vehicle";
    public static string ExamStatus => _isMacedonian ? "Статус на преглед" : "Exam Status";
    public static string CreateNewExam => _isMacedonian ? "Направи нов преглед" : "Create New Exam";

    // Column headers (short labels)
    public static string RegNumberShort => _isMacedonian ? "Рег. број" : "Reg. No.";
    public static string DateLabel => _isMacedonian ? "Датум" : "Date";
    public static string TypeLabel => _isMacedonian ? "Тип" : "Type";
    public static string CreatedLabel => _isMacedonian ? "Креирано" : "Created";
    public static string EndedLabel => _isMacedonian ? "Завршено" : "Ended";

    // Language toggle
    public static string SwitchToEnglish => "English";
    public static string SwitchToMacedonian => "Македонски";

    // Delete confirmation
    public static string ConfirmDelete => _isMacedonian ? "Дали сте сигурни дека сакате да го избришете овој запис?" : "Are you sure you want to delete this record?";
    public static string ConfirmDeleteTitle => _isMacedonian ? "Потврда за бришење" : "Confirm Delete";
    public static string SaveChangesQuestion => _isMacedonian ? "Имате направено измени, дали сакате да ги зачувате?" : "You have unsaved changes. Do you want to save?";

    // Validation
    public static string FieldRequired => _isMacedonian ? "Полето е задолжително" : "This field is required";
    public static string SavedSuccessfully => _isMacedonian ? "Успешно зачувано" : "Saved successfully";
    public static string DeletedSuccessfully => _isMacedonian ? "Успешно избришано" : "Deleted successfully";
    public static string Error => _isMacedonian ? "Грешка" : "Error";

    // Login
    public static string SignIn => _isMacedonian ? "Најави се" : "Sign In";
    public static string WelcomeBack => _isMacedonian ? "Добредојдовте" : "Welcome Back";
    public static string SystemSubtitle => _isMacedonian ? "Систем за технички преглед на возила" : "Vehicle Technical Examination System";
    public static string TechnicalExamination => _isMacedonian ? "Технички преглед" : "Technical Examination";

    // Cash Register & Payment by Category reports
    public static string CashRegister => _isMacedonian ? "Касов извештај" : "Cash Register Report";
    public static string PaymentByCategory => _isMacedonian ? "Извештај по категорија" : "Payment by Category";
    public static string Category => _isMacedonian ? "Категорија" : "Category";
    public static string Count => _isMacedonian ? "Број" : "Count";
    public static string TotalAmount => _isMacedonian ? "Вкупен износ" : "Total Amount";
    public static string VATAmount => _isMacedonian ? "ДДВ износ" : "VAT Amount";
    public static string NetAmount => _isMacedonian ? "Нето износ" : "Net Amount";
    public static string PaidDocuments => _isMacedonian ? "Платени документи" : "Paid Documents";
    public static string UnpaidDocuments => _isMacedonian ? "Неплатени документи" : "Unpaid Documents";
    public static string CancelledDocuments => _isMacedonian ? "Сторнирани" : "Cancelled";
    public static string GrandTotal => _isMacedonian ? "Вкупно:" : "Grand Total:";

    // Permissions
    public static string PermissionsTitle => _isMacedonian ? "Дозволи" : "Permissions";
    public static string NewPermission => _isMacedonian ? "+ Нова дозвола" : "+ New Permission";
    public static string PermissionNumber => _isMacedonian ? "Број на дозвола" : "Permission Number";
    public static string EditPermission => _isMacedonian ? "Измени дозвола" : "Edit Permission";
    public static string NewPermissionTitle => _isMacedonian ? "Нова дозвола" : "New Permission";

    // Relations
    public static string Relations => _isMacedonian ? "Врски клиент-возило" : "Customer-Vehicle Relations";
    public static string NewRelation => _isMacedonian ? "+ Нова врска" : "+ New Relation";
    public static string EditRelation => _isMacedonian ? "Измени врска" : "Edit Relation";
    public static string NewRelationTitle => _isMacedonian ? "Нова врска" : "New Relation";
    public static string RelationTypeField => _isMacedonian ? "Тип на врска" : "Relation Type";
    public static string StartDate => _isMacedonian ? "Почеток" : "Start Date";
    public static string EndDate => _isMacedonian ? "Крај" : "End Date";
    public static string BeginNote => _isMacedonian ? "Белешка за почеток" : "Begin Note";
    public static string TerminationNote => _isMacedonian ? "Белешка за прекин" : "Termination Note";

    // Traffic Licenses
    public static string TrafficLicenses => _isMacedonian ? "Сообраќајни дозволи" : "Traffic Licenses";
    public static string NewTrafficLicense => _isMacedonian ? "+ Нова сообраќајна" : "+ New Traffic License";
    public static string LicenseNumber => _isMacedonian ? "Број на дозвола" : "License Number";
    public static string PlateNumber => _isMacedonian ? "Регистарска табличка" : "Plate Number";
    public static string IssuedDate => _isMacedonian ? "Датум на издавање" : "Issued Date";
    public static string EditTrafficLicense => _isMacedonian ? "Измени сообраќајна дозвола" : "Edit Traffic License";
    public static string NewTrafficLicenseTitle => _isMacedonian ? "Нова сообраќајна дозвола" : "New Traffic License";

    // International Driving Licenses
    public static string IntlDrivingLicenses => _isMacedonian ? "Меѓународни возачки дозволи" : "International Driving Licenses";
    public static string NewIntlLicense => _isMacedonian ? "+ Нова меѓународна" : "+ New Intl. License";
    public static string EditIntlLicense => _isMacedonian ? "Измени меѓународна дозвола" : "Edit Intl. License";
    public static string NewIntlLicenseTitle => _isMacedonian ? "Нова меѓународна дозвола" : "New Intl. License";

    // Print / Company
    public static string Print => _isMacedonian ? "Печати" : "Print";
    public static string CompanyManagement => _isMacedonian ? "Организации" : "Organizations";
    public static string NewCompany => _isMacedonian ? "+ Нова организација" : "+ New Organization";
    public static string CompanyNameField => _isMacedonian ? "Назив" : "Name";
    public static string CompanyAddress => _isMacedonian ? "Адреса" : "Address";
    public static string SaveFirst => _isMacedonian ? "Прво зачувајте го записот" : "Save the record first";

    // PDF / Print
    public static string PrintReport => _isMacedonian ? "Печати извештај" : "Print Report";
    public static string PrintCertificate => _isMacedonian ? "Печати потврда" : "Print Certificate";
    public static string PrintReceipt => _isMacedonian ? "Печати сметка" : "Print Receipt";
    public static string PrintInvoice => _isMacedonian ? "Печати фактура" : "Print Invoice";
    public static string CertificateTitle => _isMacedonian ? "ПОТВРДА ЗА ТЕХНИЧКА ИСПРАВНОСТ" : "CERTIFICATE OF TECHNICAL CORRECTNESS";
    public static string TrafficLicenseTitle => _isMacedonian ? "СООБРАЌАЈНА ДОЗВОЛА" : "TRAFFIC LICENSE";
    public static string ReceiptTitle => _isMacedonian ? "СМЕТКА" : "RECEIPT";
    public static string InvoiceTitle => _isMacedonian ? "ФАКТУРА" : "INVOICE";
    public static string Operator => _isMacedonian ? "Оператор" : "Operator";
    public static string VehicleCorrect => _isMacedonian ? "ИСПРАВНО" : "PASSED";
    public static string VehicleIncorrect => _isMacedonian ? "НЕИСПРАВНО" : "FAILED";

    // Theme
    public static string LightMode => _isMacedonian ? "\u2600 Светла тема" : "\u2600 Light Mode";
    public static string DarkMode => _isMacedonian ? "\u263D Темна тема" : "\u263D Dark Mode";
}
