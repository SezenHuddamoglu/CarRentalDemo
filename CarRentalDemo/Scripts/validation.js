class ValidationManager {
    constructor() {
        this.boundElements = new WeakSet();
        this.init();
    }

    init() {
        this.injectValidationStyles();
        this.setupEventDelegation();
        this.disableHTML5Validation();
        this.overrideASPValidation();
        this.startCleanupInterval();
    }

    setupEventDelegation() {
        document.addEventListener('submit', (e) => {
            if (e.target.tagName === 'FORM') {
                if (!this.validateForm(e.target)) {
                    e.preventDefault();
                    e.stopPropagation();
                }
            }
        });

        document.addEventListener('blur', (e) => {
            const target = e.target;

            if (target.matches('input[type="email"], input[id*="Email"], input[name*="email"]')) {
                this.validateEmail(target);
            } else if (target.matches('input[type="password"], input[id*="Password"], input[name*="password"]')) {
                this.validatePassword(target);
            } else if (target.matches('input[type="text"][data-required], input[type="text"][required], input[data-required]')) {
                this.validateRequired(target);
            }
        }, true);

        document.addEventListener('input', (e) => {
            if (e.target.matches('input[type="email"], input[type="password"], input[type="text"]')) {
                this.clearValidationMessage(e.target);
            }
        }, true);

        document.addEventListener('click', (e) => {
            if (e.target.matches('.lang-btn')) {
                setTimeout(() => this.cleanupASPValidation(), 300);
            }
        });
    }
    cleanupASPValidation() {
        const validationSelectors = [
            'span[class*="validator"]',
            'span[id*="rfv"]',
            'span[id*="rev"]',
            'span[id*="cv"]',
            '.field-validation-error',
            '.validation-alert',
            '[data-val]',
            '[class*="validation-error"]'
        ];

        validationSelectors.forEach(selector => {
            document.querySelectorAll(selector).forEach(element => {
                if (!element.closest('.custom-validation-message')) {
                    element.remove();
                }
            });
        });

        this.resetFormElements();
    }

    resetFormElements() {
        document.querySelectorAll('.input-wrapper').forEach(wrapper => {
            Object.assign(wrapper.style, {
                display: 'flex',
                visibility: 'visible',
                opacity: '1',
                height: '44px',
                minHeight: '44px'
            });
        });

        document.querySelectorAll('.input-wrapper input').forEach(input => {
            Object.assign(input.style, {
                display: 'block',
                visibility: 'visible',
                opacity: '1',
                position: 'static',
                height: '44px',
                minHeight: '44px',
                width: '100%'
            });
        });

        
        document.querySelectorAll('.form-group').forEach(group => {
            Object.assign(group.style, {
                display: 'flex',
                visibility: 'visible',
                opacity: '1',
                height: 'auto',
                minHeight: 'auto'
            });
        });
    }

    disableHTML5Validation() {
        document.querySelectorAll('form').forEach(form => {
            form.setAttribute('novalidate', 'novalidate');
            form.noValidate = true;
        });
    }

    overrideASPValidation() {
        const overrides = {
            ValidatorEnable: () => true,
            Page_ClientValidate: () => true,
            ValidatorUpdateDisplay: () => { }
        };

        Object.entries(overrides).forEach(([funcName, func]) => {
            if (typeof window[funcName] !== 'undefined') {
                window[funcName] = func;
            }
        });

        if (typeof Page_Validators !== 'undefined') {
            Page_Validators.length = 0;
        }
    }

    startCleanupInterval() {
        setInterval(() => this.cleanupASPValidation(), 300);
    }

    
    validateEmail(input) {
        const email = input.value.trim();
        const emailRegex = /^[a-zA-Z0-9._%+-]+@[a-zA-Z0-9.-]+\.[a-zA-Z]{2,}$/;

        this.cleanupASPValidation();

        if (!email) {
            this.showValidationMessage(input, this.getLocalizedMessage("emailRequired"), "error");
            return false;
        }

        if (!emailRegex.test(email)) {
            this.showValidationMessage(input, this.getLocalizedMessage("emailInvalid"), "error");
            return false;
        }

        this.clearValidationMessage(input);
        return true;
    }
    validatePassword(input) {
        const password = input.value;

        const passwordRegex = /^(?=.*[a-z])(?=.*[A-Z])(?=.*\d).{6,}$/;

        this.cleanupASPValidation();

        if (!password) {
            this.showValidationMessage(input, this.getLocalizedMessage("passwordRequired"), "error");
            return false;
        }

        if (!passwordRegex.test(password)) {
            this.showValidationMessage(input, this.getLocalizedMessage("passwordInvalid"), "error");
            return false;
        }

        this.clearValidationMessage(input);
        return true;
   }
    validateRequired(input) {
        const value = input.value.trim();

        this.cleanupASPValidation();

        if (!value) {
            this.showValidationMessage(input, this.getLocalizedMessage("fieldRequired"), "error");
            return false;
        }

        if (input.id.includes("FullName")) {
            const nameRegex = /^[a-zA-ZçğıöşüÇĞIİÖŞÜ\s]{2,50}$/;
            if (!nameRegex.test(value)) {
                this.showValidationMessage(input, this.getLocalizedMessage("nameInvalid"), "error");
                return false;
            }
        }

        this.clearValidationMessage(input);
        return true;
    }

    validateForm(form) {
        let isValid = true;

        this.cleanupASPValidation();

        const inputs = form.querySelectorAll('input[type="email"], input[type="password"], input[type="text"][data-required], input[type="text"][required]');

        inputs.forEach(input => {
            let fieldValid = true;

            if (input.type === "email" || input.id.includes("txtEmail")) {
                fieldValid = this.validateEmail(input);
            } else if (input.type === "password") {
                fieldValid = this.validatePassword(input);
            } else if (input.type === "text") {
                fieldValid = this.validateRequired(input);
            }

            if (!fieldValid) isValid = false;
        });

        if (!isValid) {
            this.showFormError(this.getLocalizedMessage("formValidationError"));
            return false;
        }

        return true;
    }

    showValidationMessage(input, message, type = "error") {
        this.cleanupASPValidation();

        input.classList.add("error");

        const formGroup = input.closest('.form-group');
        if (!formGroup) return;

        let messageDiv = formGroup.querySelector('.custom-validation-message');

        if (!messageDiv) {
            messageDiv = document.createElement("div");
            messageDiv.className = "custom-validation-message";
            formGroup.appendChild(messageDiv);
        }

        messageDiv.innerHTML = `
            <div class="validation-content ${type}">
                <svg class="validation-icon" width="14" height="14" viewBox="0 0 24 24" fill="currentColor">
                    <path d="M12 2C6.48 2 2 6.48 2 12s4.48 10 10 10 10-4.48 10-10S17.52 2 12 2zm1 15h-2v-2h2v2zm0-4h-2V7h2v6z"/>
                </svg>
                <span class="validation-text">${message}</span>
            </div>
        `;
        messageDiv.classList.add("show");
    }

    clearValidationMessage(input) {
        const formGroup = input.closest('.form-group');
        const messageDiv = formGroup?.querySelector('.custom-validation-message');

        if (messageDiv) {
            messageDiv.classList.remove("show");
            messageDiv.innerHTML = "";
            input.classList.remove("error");
        }
    }

    showFormError(message) {
        let errorDiv = document.querySelector(".form-error-message");

        if (!errorDiv) {
            errorDiv = document.createElement("div");
            errorDiv.className = "form-error-message";

            const container = document.querySelector(".auth-container");
            if (container) {
                container.insertBefore(errorDiv, container.firstChild);
            }
        }

        errorDiv.innerHTML = `
            <div class="alert alert-error">
                <svg width="18" height="18" viewBox="0 0 24 24" fill="currentColor">
                    <path d="M12 2C6.48 2 2 6.48 2 12s4.48 10 10 10 10-4.48 10-10S17.52 2 12 2zm1 15h-2v-2h2v2zm0-4h-2V7h2v6z"/>
                </svg>
                <span>${message}</span>
            </div>
        `;

        errorDiv.style.display = "block";

        setTimeout(() => {
            if (errorDiv) {
                errorDiv.style.display = "none";
            }
        }, 5000);
    }

    getCurrentLanguage() {
        const turkishBtn = document.querySelector('[id*="Turkish"]');
        const englishBtn = document.querySelector('[id*="English"]');

        if (turkishBtn?.classList.contains('active') || turkishBtn?.style.fontWeight === 'bold') return 'tr';
        if (englishBtn?.classList.contains('active') || englishBtn?.style.fontWeight === 'bold') return 'en';

        return document.documentElement.lang || "tr";
    }

    getLocalizedMessage(key) {
        const lang = this.getCurrentLanguage();
        const messages = {
            tr: {
                emailRequired: "E-posta adresi gereklidir.",
                emailInvalid: "Geçerli bir e-posta adresi giriniz.",
                passwordRequired: "Şifre gereklidir.",
                passwordInvalid: "Şifre en az 6 karakter olmalı <br> Büyük harf, küçük harf ve rakam içermelidir.",
                fieldRequired: "Bu alan gereklidir.",
                nameInvalid: "Geçerli bir ad soyad giriniz.",
                formValidationError: "Lütfen tüm alanları doğru şekilde doldurunuz.",
            },
            en: {
                emailRequired: "Email address is required.",
                emailInvalid: "Please enter a valid email address.",
                passwordRequired: "Password is required.",
                passwordInvalid: "Password must be at least 6 characters<br> Contain uppercase, lowercase and number.",
                fieldRequired: "This field is required.",
                nameInvalid: "Please enter a valid full name.",
                formValidationError: "Please fill all fields correctly.",
            },
        };

        return messages[lang]?.[key] || messages["tr"][key] || key;
    }

    injectValidationStyles() {
        if (document.querySelector('#validation-styles')) return;

        const style = document.createElement("style");
        style.id = 'validation-styles';
        style.textContent = `
            span[class*="validator"],
            span[id*="rfv"],
            span[id*="rev"], 
            span[id*="cv"],
            .field-validation-error,
            .validation-alert,
            [data-val] {
                display: none !important;
                visibility: hidden !important;
                opacity: 0 !important;
                width: 0 !important;
                height: 0 !important;
                position: absolute !important;
                left: -9999px !important;
            }

            .auth-page .input-wrapper {
                position: relative !important;
                width: 100% !important;
                display: flex !important;
                align-items: center !important;
                visibility: visible !important;
                opacity: 1 !important;
                height: 44px !important;
                min-height: 44px !important;
            }

            .auth-page .form-input {
                box-sizing: border-box !important;
                min-height: 44px !important;
                height: 44px !important;
                width: 100% !important;
                visibility: visible !important;
                opacity: 1 !important;
                display: block !important;
                position: relative !important;
            }

            .auth-page .form-group {
                margin-bottom: 20px !important;
                position: relative !important;
                display: flex !important;
                flex-direction: column !important;
                min-height: auto !important;
                height: auto !important;
                overflow: visible !important;
            }

            .custom-validation-message {
                opacity: 0;
                max-height: 0;
                overflow: hidden;
                transition: opacity 0.3s ease, max-height 0.3s ease;
                margin-top: 6px;
                width: 100%;
                position: static !important;
                height: auto !important;
                visibility: visible !important;
            }

            .custom-validation-message.show {
                opacity: 1;
                max-height: 50px !important;
                position: static !important;
            }

            .validation-content {
                display: flex;
                align-items: flex-start;
                gap: 6px;
                padding: 6px 10px;
                border-radius: 4px;
                font-size: 12px;
                font-weight: 500;
                line-height: 1.3;
                animation: slideIn 0.3s ease;
                word-wrap: break-word;
                white-space: normal;
                width: 100%;
                box-sizing: border-box;
                position: static !important;
            }

            .validation-content.error {
                background: linear-gradient(135deg, #fef2f2 0%, #fee2e2 100%);
                color: #dc2626;
                border: 1px solid #fecaca;
            }

            .auth-page .form-input.error {
                border-color: #ef4444 !important;
                box-shadow: 0 0 0 2px rgba(239, 68, 68, 0.1) !important;
                background: rgba(255, 255, 255, 0.95) !important;
            }

            @keyframes slideIn {
                from {
                    opacity: 0;
                    transform: translateY(-5px);
                }
                to {
                    opacity: 1;
                    transform: translateY(0);
                }
            }

            input[type="hidden"],
            .asp-net-hidden {
                display: none !important;
            }
        `;
        document.head.appendChild(style);
    }
}


document.addEventListener("DOMContentLoaded", () => {
    new ValidationManager();
});