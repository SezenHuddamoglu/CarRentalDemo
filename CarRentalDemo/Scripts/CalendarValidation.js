class CalendarValidationManager {
    constructor() {
        this.currentLang = 'tr';
        this.init();
    }

    init() {
        this.setupEventDelegation();
        this.initializeDatePickers();
        this.handleNavigationCleanup();
        this.setupUpdatePanelHandling();
    }

   
    setupEventDelegation() {
        document.addEventListener('click', (e) => {
            if (e.target.closest('.lang-btn')) {
                this.currentLang = this.getCurrentLanguage();
                setTimeout(() => this.initializeDatePickers(), 500);
            }
        });

        
        document.addEventListener('change', (e) => {
            const target = e.target;

            if (target.matches('#ddlVehicleType')) {
                sessionStorage.setItem('vehicleTypeChanged', 'true');
                this.clearSingleValidation(target);
            } else if (target.matches('#ddlVehicleModel')) {
                this.clearSingleValidation(target);
            }
        });

        
        document.addEventListener('click', (e) => {
            if (e.target.matches('#btnClear')) {
                this.clearAllValidationMessages();
            }
        });

        
        document.addEventListener('click', (e) => {
            if (e.target.matches('a[href*="RentalList.aspx"], a[href*="Login.aspx"], .logo')) {
                sessionStorage.setItem('crossPageNavigation', 'true');
            }
        });

        
        document.addEventListener('blur', (e) => {
            if (e.target.matches('#txtStart, #txtEnd')) {
                this.validateManualInput(e.target);
            }
        });

        
        document.addEventListener('click', (e) => {
            if (!e.target.closest('.hasDatepicker, .ui-datepicker, .ui-datepicker-header')) {
                document.querySelectorAll('.hasDatepicker').forEach(input => {
                    $(input).datepicker('hide');
                });
            }
        });

        
        document.addEventListener('click', (e) => {
            if (e.target.closest('.ui-datepicker-prev, .ui-datepicker-next')) {
                e.stopPropagation();
            }
        });

        document.addEventListener('change', (e) => {
            if (e.target.matches('.ui-datepicker-month, .ui-datepicker-year')) {
                e.stopPropagation();
            }
        });
    }

    handleNavigationCleanup() {
        const navigationEntry = performance.getEntriesByType('navigation')[0];
        if (navigationEntry?.type === 'reload' || sessionStorage.getItem('pageJustRefreshed') === 'true') {
            this.clearAllValidationMessages();
            sessionStorage.removeItem('pageJustRefreshed');
            return;
        }

        
        if ((document.referrer && !document.referrer.includes('RentalRequest.aspx') &&
            document.referrer !== '' && document.referrer !== window.location.href) ||
            sessionStorage.getItem('crossPageNavigation') === 'true') {
            this.clearAllValidationMessages();
            sessionStorage.removeItem('crossPageNavigation');
        }

        
        window.addEventListener('beforeunload', () => {
            if (!this.isPostbackInProgress()) {
                if (window.location.href.includes('RentalRequest.aspx')) {
                    sessionStorage.setItem('crossPageNavigation', 'true');
                }
                sessionStorage.setItem('pageJustRefreshed', 'true');
            }
        });
    }

    isPostbackInProgress() {
        const form = document.forms[0];
        return !!(form?.__EVENTTARGET || form?.__EVENTARGUMENT);
    }

    initializeDatePickers() {
        this.currentLang = this.getCurrentLanguage();
        const datepickerOptions = this.getDatepickerOptions();

        
        this.destroyDatepickers('#txtStart, #txtEnd');

        
        $('#txtStart').datepicker({
            ...datepickerOptions,
            onSelect: (selectedDate) => {
                const startDate = this.parseDate(selectedDate);
                if (!this.validateDate(startDate, 'start')) return;

                this.updateEndDateRange(startDate);
                this.adjustInvalidEndDate(startDate);
                this.clearSingleValidation('#txtStart');
            }
        });

        
        $('#txtEnd').datepicker({
            ...datepickerOptions,
            onSelect: (selectedDate) => {
                const endDate = this.parseDate(selectedDate);
                const startDate = this.parseDate($('#txtStart').val());

                this.validateDateRange(startDate, endDate);
                this.clearSingleValidation('#txtEnd');
            }
        });

        
        if (sessionStorage.getItem('vehicleTypeChanged') === 'true') {
            setTimeout(() => {
                this.clearSingleValidation('#ddlVehicleType');
                sessionStorage.removeItem('vehicleTypeChanged');
            }, 100);
        }

        
        const tomorrow = new Date();
        tomorrow.setDate(tomorrow.getDate() + 1);
        $('#txtEnd').datepicker('option', 'minDate', tomorrow);
    }

    
    validateDate(date, type = 'start') {
        const today = new Date();
        today.setHours(0, 0, 0, 0);

        if (!date) return false;

        if (date < today) {
            const message = this.currentLang === 'tr'
                ? 'Başlangıç tarihi bugünden önce olamaz!'
                : 'Start date cannot be before today!';
            this.showMessage('#txtStart', message, 'error');
            $('#txtStart').val('');
            return false;
        }

        return true;
    }

    validateDateRange(startDate, endDate) {
        if (!startDate || !endDate) return true;

        const today = new Date();
        today.setHours(0, 0, 0, 0);

        
        if (endDate <= startDate) {
            const message = this.currentLang === 'tr'
                ? 'Bitiş tarihi başlangıç tarihinden önce olamaz!'
                : 'End date cannot be before start date!';
            this.showMessage('#txtEnd', message, 'error');

            const newEnd = new Date(startDate);
            newEnd.setDate(newEnd.getDate() + 1);
            $('#txtEnd').val(this.formatDate(newEnd));
            return false;
        }

        const maxEnd = new Date(startDate);
        maxEnd.setMonth(maxEnd.getMonth() + 3);
        if (endDate > maxEnd) {
            const message = this.currentLang === 'tr'
                ? 'Maksimum kiralama süresi 3 aydır!'
                : 'Maximum rental period is 3 months!';
            this.showMessage('#txtEnd', message, 'error');
            $('#txtEnd').val('');
            return false;
        }

        return true;
    }

    validateManualInput(input) {
        const value = input.value;
        if (!value) return;

        const dateRegex = /^(\d{2})\/(\d{2})\/(\d{4})$/;
        if (!dateRegex.test(value)) {
            this.showMessage(input,
                this.currentLang === 'tr' ? 'Geçersiz tarih formatı! gg/aa/yyyy olmalı.' : 'Invalid date format! Use dd/mm/yyyy.',
                'error'
            );
            return;
        }

        const date = this.parseDate(value);
        if (!date) {
            this.showMessage(input,
                this.currentLang === 'tr' ? 'Geçersiz tarih!' : 'Invalid date!',
                'error'
            );
            return;
        }

        const today = new Date();
        today.setHours(0, 0, 0, 0);
        if (date < today) {
            this.showMessage(input,
                this.currentLang === 'tr' ? 'Geçmiş tarih seçemezsiniz!' : 'You cannot select past dates!',
                'error'
            );
            $(input).val('');
            return;
        }

        if (input.id === 'txtEnd' && $('#txtStart').val()) {
            const startDate = this.parseDate($('#txtStart').val());
            if (!this.validateDateRange(startDate, date)) {
                return;
            }
        }

        this.clearSingleValidation(input);
    }

    
    clearSingleValidation(input) {
        const inputElement = typeof input === 'string'
            ? document.getElementById(input.replace('#', ''))
            : input;

        if (!inputElement) return;

        const formGroup = inputElement.closest('.form-group');
        if (!formGroup) return;

        
        formGroup.querySelectorAll('.date-error, .date-info, .validation-error, .validation-info').forEach(el => el.remove());
        inputElement.classList.remove('error', 'info');

        
        const inputId = inputElement.id;
        formGroup.querySelectorAll('[id*="cv"], [id*="rfv"]').forEach(validator => {
            if (validator.getAttribute('controltovalidate') === inputId) {
                validator.style.display = 'none';
                validator.textContent = '';
            }
        });
    }

    clearAllValidationMessages() {
        
        document.querySelectorAll('.validation-error, .validation-info, .date-error, .date-info').forEach(el => el.remove());
        document.querySelectorAll('input, select').forEach(el => el.classList.remove('error', 'info'));

        
        document.querySelectorAll('[id*="cv"], [id*="rfv"]').forEach(validator => {
            validator.style.display = 'none';
            validator.textContent = '';
        });

        
        if (typeof Page_Validators !== 'undefined') {
            Page_Validators.forEach(validator => {
                validator.isvalid = true;
                validator.style.display = 'none';
                validator.style.visibility = 'hidden';
            });
        }
    }

    showMessage(input, message, type = 'error') {
        const inputElement = typeof input === 'string'
            ? document.getElementById(input.replace('#', ''))
            : input;

        if (!inputElement) return;

        const formGroup = inputElement.closest('.form-group');
        if (!formGroup) return;

        
        formGroup.querySelectorAll('.date-error, .date-info, .validation-error, .validation-info').forEach(el => el.remove());

        
        const messageDiv = document.createElement('div');
        messageDiv.className = type === 'error' ? 'date-error validation-error' : 'date-info validation-info';
        messageDiv.textContent = message;
        formGroup.appendChild(messageDiv);

    }

    
    getCurrentLanguage() {
        const turkishBtn = document.querySelector('[id*="Turkish"]');
        const englishBtn = document.querySelector('[id*="English"]');

        if (turkishBtn?.classList.contains('active') || turkishBtn?.style.fontWeight === 'bold') return 'tr';
        if (englishBtn?.classList.contains('active') || englishBtn?.style.fontWeight === 'bold') return 'en';

        return document.documentElement.lang || 'tr';
    }

    parseDate(dateString) {
        if (!dateString) return null;
        const [d, m, y] = dateString.split('/').map(Number);
        const date = new Date(y, m - 1, d);
        return date.getFullYear() === y && date.getMonth() === m - 1 && date.getDate() === d ? date : null;
    }

    formatDate(date) {
        return date ? `${String(date.getDate()).padStart(2, '0')}/${String(date.getMonth() + 1).padStart(2, '0')}/${date.getFullYear()}` : '';
    }

    destroyDatepickers(selector) {
        document.querySelectorAll(selector).forEach(input => {
            if (input.classList.contains('hasDatepicker')) {
                $(input).datepicker('destroy');
            }
        });
    }

    updateEndDateRange(startDate) {
        const minEnd = new Date(startDate);
        minEnd.setDate(minEnd.getDate() + 1);

        const maxEnd = new Date(startDate);
        maxEnd.setMonth(maxEnd.getMonth() + 3);

        $('#txtEnd').datepicker('option', { minDate: minEnd, maxDate: maxEnd });
    }

    adjustInvalidEndDate(startDate) {
        const endInput = document.getElementById('txtEnd');
        const currentEnd = this.parseDate(endInput?.value);

        if (currentEnd && currentEnd <= startDate) {
            const newEnd = new Date(startDate);
            newEnd.setDate(newEnd.getDate() + 1);
            endInput.value = this.formatDate(newEnd);

            this.showMessage('#txtEnd',
                this.currentLang === 'tr'
                    ? 'Bitiş tarihi başlangıç tarihinden önce olamaz! Otomatik olarak düzeltildi.'
                    : 'End date cannot be before start date! It has been auto-corrected.',
                'info'
            );
        }
    }

    getDatepickerOptions() {
        const baseOptions = {
            dateFormat: 'dd/mm/yy',
            minDate: 0,
            maxDate: '+3Y',
            changeMonth: true,
            changeYear: true,
            yearRange: 'c:c+3',
            showAnim: 'fadeIn',
            constrainInput: true,
            beforeShowDay: (date) => {
                const today = new Date();
                today.setHours(0, 0, 0, 0);
                if (date < today) return [false, 'past-date disabled-date', this.currentLang === 'tr' ? 'Geçmiş tarih' : 'Past date'];
                return [true, date.getDay() === 0 || date.getDay() === 6 ? 'weekend' : ''];
            },
            onClose: function () {
                const input = this;
                setTimeout(() => {
                    window.calendarManager.validateManualInput(input);
                    window.calendarManager.clearSingleValidation(input);
                }, 10);
            }
        };

        const locales = {
            tr: {
                closeText: 'Kapat',
                prevText: '&#x3C;Önceki',
                nextText: 'Sonraki&#x3E;',
                currentText: 'Bugün',
                monthNames: ['Ocak', 'Şubat', 'Mart', 'Nisan', 'Mayıs', 'Haziran', 'Temmuz', 'Ağustos', 'Eylül', 'Ekim', 'Kasım', 'Aralık'],
                monthNamesShort: ['Oca', 'Şub', 'Mar', 'Nis', 'May', 'Haz', 'Tem', 'Ağu', 'Eyl', 'Eki', 'Kas', 'Ara'],
                dayNames: ['Pazar', 'Pazartesi', 'Salı', 'Çarşamba', 'Perşembe', 'Cuma', 'Cumartesi'],
                dayNamesShort: ['Paz', 'Pzt', 'Sal', 'Çar', 'Per', 'Cum', 'Cmt'],
                dayNamesMin: ['Pz', 'Pt', 'Sa', 'Ça', 'Pe', 'Cu', 'Ct'],
                weekHeader: 'Hf',
                firstDay: 1
            },
            en: {
                closeText: 'Close',
                prevText: 'Prev',
                nextText: 'Next',
                currentText: 'Today',
                monthNames: ['January', 'February', 'March', 'April', 'May', 'June', 'July', 'August', 'September', 'October', 'November', 'December'],
                monthNamesShort: ['Jan', 'Feb', 'Mar', 'Apr', 'May', 'Jun', 'Jul', 'Aug', 'Sep', 'Oct', 'Nov', 'Dec'],
                dayNames: ['Sunday', 'Monday', 'Tuesday', 'Wednesday', 'Thursday', 'Friday', 'Saturday'],
                dayNamesShort: ['Sun', 'Mon', 'Tue', 'Wed', 'Thu', 'Fri', 'Sat'],
                dayNamesMin: ['Su', 'Mo', 'Tu', 'We', 'Th', 'Fr', 'Sa'],
                weekHeader: 'Wk',
                firstDay: 0
            }
        };

        return { ...baseOptions, ...locales[this.currentLang] };
    }

    setupUpdatePanelHandling() {
        if (typeof Sys === 'undefined' || !Sys.WebForms || !Sys.WebForms.PageRequestManager) return;

        const prm = Sys.WebForms.PageRequestManager.getInstance();

        prm.add_beginRequest(() => {
            const errorMessages = [];
            document.querySelectorAll('.validation-error, .date-error').forEach(msg => {
                if (msg.offsetParent !== null && msg.textContent.trim() !== '') {
                    const input = msg.closest('.form-group')?.querySelector('input, select');
                    if (input) {
                        errorMessages.push({
                            text: msg.textContent.trim(),
                            for: input.id
                        });
                    }
                }
            });
            sessionStorage.setItem('pendingErrorValidations', JSON.stringify(errorMessages));
        });

        prm.add_endRequest(() => {
            const savedErrors = sessionStorage.getItem('pendingErrorValidations');
            if (savedErrors) {
                const errors = JSON.parse(savedErrors);
                errors.forEach(error => {
                    if (error.text && error.for) {
                        const input = document.getElementById(error.for);
                        if (input) {
                            this.showMessage(input, error.text, 'error');
                        }
                    }
                });
                sessionStorage.removeItem('pendingErrorValidations');
            }

            
            setTimeout(() => this.initializeDatePickers(), 100);
        });
    }
}


window.calendarManager = null;


document.addEventListener('DOMContentLoaded', () => {
    window.calendarManager = new CalendarValidationManager();
});