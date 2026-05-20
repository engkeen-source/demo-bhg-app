.PHONY: install install-backend install-frontend dev dev-backend dev-frontend help

BACKEND_DIR  := backend
FRONTEND_DIR := Web
VENV         := $(BACKEND_DIR)/.venv
PYTHON       := $(VENV)/bin/python
PIP          := $(VENV)/bin/pip
UVICORN      := $(VENV)/bin/uvicorn

# ── Install ───────────────────────────────────────────────────────────────────

install: install-backend install-frontend  ## Install all dependencies

install-backend:  ## Create venv and install Python packages
	@echo "→ Installing backend dependencies…"
	python3 -m venv $(VENV)
	$(PIP) install --upgrade pip -q
	$(PIP) install -r $(BACKEND_DIR)/requirements.txt
	@if [ ! -f $(BACKEND_DIR)/.env ]; then \
		cp $(BACKEND_DIR)/.env.example $(BACKEND_DIR)/.env; \
		echo "  ✔ Created backend/.env from .env.example — fill in DATABASE_URL"; \
	else \
		echo "  ✔ backend/.env already exists"; \
	fi
	@echo "  ✔ Backend ready"

install-frontend:  ## Install Node packages
	@echo "→ Installing frontend dependencies…"
	cd $(FRONTEND_DIR) && npm install
	@echo "  ✔ Frontend ready"

# ── Dev ───────────────────────────────────────────────────────────────────────

dev:  ## Run backend (port 8000) + frontend (port 3000) in parallel
	@echo "→ Starting backend + frontend…"
	@trap 'kill 0' INT; \
		$(MAKE) dev-backend & \
		$(MAKE) dev-frontend & \
		wait

dev-backend:  ## Run FastAPI on port 8000
	@echo "→ Backend  →  http://localhost:8000  (docs: http://localhost:8000/docs)"
	cd $(BACKEND_DIR) && $(UVICORN) app.main:app --reload --port 8000

dev-frontend:  ## Run Next.js on port 3000
	@echo "→ Frontend →  http://localhost:3000"
	cd $(FRONTEND_DIR) && npm run dev

# ── Help ─────────────────────────────────────────────────────────────────────

help:  ## Show this help
	@grep -E '^[a-zA-Z_-]+:.*##' $(MAKEFILE_LIST) | \
		awk 'BEGIN {FS = ":.*##"}; {printf "  \033[36m%-20s\033[0m %s\n", $$1, $$2}'
