(function () {
  'use strict';

  function wireReveal(input) {
    var box = input.closest('.m-input');
    if (!box || box.querySelector('.ms-reveal')) return;

    var drawn = box.querySelector('i.bi-eye, i.bi-eye-slash');
    var btn = document.createElement('button');
    btn.type = 'button';
    btn.className = 'ms-reveal';
    btn.setAttribute('aria-label', 'Show the password');
    btn.innerHTML = '<i class="bi bi-eye"></i>';

    if (drawn) {
      btn.innerHTML = '<i class="' + drawn.className + '"></i>';
      drawn.replaceWith(btn);
    } else {
      box.appendChild(btn);
    }

    btn.addEventListener('click', function () {
      var show = input.type === 'password';
      input.type = show ? 'text' : 'password';
      btn.querySelector('i').className = 'bi ' + (show ? 'bi-eye-slash' : 'bi-eye');
      btn.setAttribute('aria-label', show ? 'Hide the password' : 'Show the password');
      input.focus({ preventScroll: true });
    });
  }

  function wireAllReveals(root) {
    (root || document).querySelectorAll('input[type=password]').forEach(wireReveal);
  }

  var confirmBox = null;

  function buildConfirm() {
    if (confirmBox) return confirmBox;

    confirmBox = document.createElement('div');
    confirmBox.className = 'ms-dialog';
    confirmBox.id = 'ms-confirm-dialog';
    confirmBox.hidden = true;
    confirmBox.innerHTML =
      '<div class="card pad ms-confirm" style="width:460px">' +
      '<div class="ms-confirm-ic"><i class="bi bi-question-lg"></i></div>' +
      '<h3 class="m-h3" data-ms-c-title>Are you sure?</h3>' +
      '<p class="m-body" data-ms-c-text></p>' +
      '<div class="ms-confirm-actions">' +
      '<button type="button" class="m-btn m-btn-outline" data-ms-close>Keep it</button>' +
      '<button type="button" class="m-btn m-btn-primary" data-ms-c-go>Yes, continue</button>' +
      '</div></div>';
    document.body.appendChild(confirmBox);

    confirmBox.addEventListener('click', function (e) {
      if (e.target === confirmBox || e.target.closest('[data-ms-close]')) close();
    });

    return confirmBox;
  }

  function close() {
    if (confirmBox) confirmBox.hidden = true;
  }

  var TONES = {
    danger: { ic: 'bi-trash', cls: 'danger', btn: 'm-btn-danger' },
    warn: { ic: 'bi-exclamation-triangle', cls: 'warn', btn: 'm-btn-primary' },
    logout: { ic: 'bi-box-arrow-right', cls: 'warn', btn: 'm-btn-primary' },
    ask: { ic: 'bi-question-lg', cls: '', btn: 'm-btn-primary' }
  };

  function ask(opts, run) {
    var box = buildConfirm();
    var tone = TONES[opts.tone] || TONES.ask;

    var ic = box.querySelector('.ms-confirm-ic');
    ic.className = 'ms-confirm-ic ' + tone.cls;
    ic.innerHTML = '<i class="bi ' + tone.ic + '"></i>';

    box.querySelector('[data-ms-c-title]').textContent = opts.title || 'Are you sure?';
    box.querySelector('[data-ms-c-text]').textContent = opts.text || '';

    var go = box.querySelector('[data-ms-c-go]');
    go.className = 'm-btn ' + tone.btn;
    go.textContent = opts.confirm || 'Yes, continue';
    box.querySelector('[data-ms-close]').textContent = opts.cancel || 'Keep it';

    var fresh = go.cloneNode(true);
    go.replaceWith(fresh);
    fresh.addEventListener('click', function () { close(); run(); });

    box.hidden = false;
    fresh.focus({ preventScroll: true });
  }

  function readOpts(el) {
    return {
      title: el.getAttribute('data-ms-confirm-title'),
      text: el.getAttribute('data-ms-confirm') || el.getAttribute('data-ms-confirm-text'),
      confirm: el.getAttribute('data-ms-confirm-ok'),
      cancel: el.getAttribute('data-ms-confirm-cancel'),
      tone: el.getAttribute('data-ms-confirm-tone') || 'ask'
    };
  }

  function send(form, submitter) {
    form.dataset.msConfirmed = '1';
    if (submitter && typeof form.requestSubmit === 'function') { form.requestSubmit(submitter); return; }
    if (typeof form.requestSubmit === 'function') { form.requestSubmit(); return; }
    form.submit();
  }

  document.addEventListener('submit', function (e) {
    var form = e.target;
    if (!form.hasAttribute || !form.hasAttribute('data-ms-confirm')) return;
    if (form.dataset.msConfirmed === '1') return;

    var submitter = e.submitter || null;
    e.preventDefault();
    ask(readOpts(form), function () { send(form, submitter); });
  });

  document.addEventListener('click', function (e) {
    var el = e.target.closest('a[data-ms-confirm], button[data-ms-confirm]');
    if (!el || el.closest('form[data-ms-confirm]')) return;

    var form = el.form || (el.type === 'submit' ? el.closest('form') : null);
    e.preventDefault();

    ask(readOpts(el), function () {
      if (form) {
        send(form, el.type === 'submit' ? el : null);
      } else if (el.href) {
        window.location = el.href;
      }
    });
  });

  document.addEventListener('keydown', function (e) {
    if (e.key === 'Escape') close();
  });

  var header = document.querySelector('.ms-landing .m-header');
  if (header) {
    var onScroll = function () {
      header.classList.toggle('is-stuck', window.scrollY > 40);
    };
    window.addEventListener('scroll', onScroll, { passive: true });
    onScroll();
  }

  document.querySelectorAll('.ms-marquee-track').forEach(function (track) {
    if (track.dataset.msCloned === '1') return;
    track.dataset.msCloned = '1';

    track.innerHTML += track.innerHTML;
  });

  var toastWrap = null;

  var TOAST = {
    success: { ic: 'bi-check-circle-fill', life: 4500 },
    error: { ic: 'bi-exclamation-octagon-fill', life: 7000 },
    warn: { ic: 'bi-exclamation-triangle-fill', life: 6000 },
    info: { ic: 'bi-info-circle-fill', life: 5000 }
  };

  function toastHost() {
    if (toastWrap && document.body.contains(toastWrap)) return toastWrap;
    toastWrap = document.createElement('div');
    toastWrap.className = 'ms-toasts';
    toastWrap.setAttribute('role', 'status');
    toastWrap.setAttribute('aria-live', 'polite');
    document.body.appendChild(toastWrap);
    return toastWrap;
  }

  function drop(el) {
    if (!el || el.dataset.msGoing === '1') return;
    el.dataset.msGoing = '1';
    el.classList.add('is-going');
    setTimeout(function () { if (el.parentNode) el.parentNode.removeChild(el); }, 260);
  }

  function toast(tone, text) {
    if (!text) return null;
    var kind = TOAST[tone] ? tone : 'info';
    var cfg = TOAST[kind];

    var el = document.createElement('div');
    el.className = 'ms-toast ' + kind;
    el.innerHTML = '<i class="bi ' + cfg.ic + '"></i><p></p>'
      + '<button type="button" class="ms-toast-x" aria-label="Close"><i class="bi bi-x-lg"></i></button>'
      + '<span class="ms-toast-life"></span>';
    el.querySelector('p').textContent = text;

    var host = toastHost();

    while (host.children.length >= 4) drop(host.firstElementChild);
    host.appendChild(el);

    var life = el.querySelector('.ms-toast-life');
    life.style.animationDuration = cfg.life + 'ms';

    var timer = setTimeout(function () { drop(el); }, cfg.life);
    el.querySelector('.ms-toast-x').addEventListener('click', function () { clearTimeout(timer); drop(el); });
    el.addEventListener('mouseenter', function () { clearTimeout(timer); life.style.animationPlayState = 'paused'; });
    el.addEventListener('mouseleave', function () {
      life.style.animationPlayState = 'running';
      timer = setTimeout(function () { drop(el); }, 2200);
    });

    return el;
  }

  window.msToast = toast;

  document.querySelectorAll('[data-ms-toast]').forEach(function (seed) {
    toast(seed.getAttribute('data-ms-toast'), seed.textContent.trim());
  });

  document.querySelectorAll('.m-bars').forEach(function (bars) {
    var scope = bars.closest('.m-fg') || bars.parentElement;
    var input = scope && scope.querySelector('input[type=password]');
    if (!input) {
      var form = bars.closest('form');
      input = form && form.querySelector('input[type=password]');
    }
    if (!input) return;

    var cells = bars.querySelectorAll('span');

    function paint() {
      var v = input.value;
      var met = [v.length >= 8, /[a-z]/.test(v), /[A-Z]/.test(v), /\d/.test(v)]
        .filter(Boolean).length;

      if (met === 4 && v.length < 12) met = 3;

      cells.forEach(function (cell, i) {
        cell.className = i < met ? (met >= 3 ? 'on2' : 'on') : '';
      });
      bars.setAttribute('data-ms-strength', v ? met : 0);
    }

    input.addEventListener('input', paint);
    paint();
  });

  var top = document.querySelector('[data-ms-top]');
  if (top) {
    var onTop = function () { top.classList.toggle('is-on', window.scrollY > 520); };
    window.addEventListener('scroll', onTop, { passive: true });
    top.addEventListener('click', function (e) {
      e.preventDefault();
      window.scrollTo({ top: 0, behavior: 'smooth' });
    });
    onTop();
  }

  function closeAllLists(except) {
    document.querySelectorAll('.ms-sel-list:not([hidden])').forEach(function (l) {
      if (l === except) return;
      l.hidden = true;
      l.parentElement.classList.remove('is-open');
      if (l.msFace) l.msFace.setAttribute('aria-expanded', 'false');
      if (l.msFollow) {
        window.removeEventListener('scroll', l.msFollow, true);
        window.removeEventListener('resize', l.msFollow);
        l.msFollow = null;
      }
    });
  }

  function buildSelect(sel) {
    if (sel.dataset.msPretty === '1' || sel.multiple || sel.size > 1) return;
    if (sel.hasAttribute('data-ms-plain')) return;
    var box = sel.closest('.m-input');
    if (!box) return;

    sel.dataset.msPretty = '1';
    box.classList.add('ms-sel');

    var face = document.createElement('button');
    face.type = 'button';
    face.className = 'ms-sel-face';
    face.setAttribute('aria-haspopup', 'listbox');
    face.setAttribute('aria-expanded', 'false');
    if (sel.id) face.setAttribute('aria-labelledby', sel.id + '-label');
    face.innerHTML = '<span class="t"></span><i class="bi bi-chevron-down"></i>';

    var list = document.createElement('div');
    list.className = 'ms-sel-list';
    list.setAttribute('role', 'listbox');
    list.hidden = true;

    function fill() {
      list.innerHTML = '';
      Array.prototype.forEach.call(sel.options, function (opt, i) {
        var row = document.createElement('div');
        row.className = 'ms-sel-opt';
        row.setAttribute('role', 'option');
        row.dataset.i = i;
        if (opt.disabled) row.classList.add('off');
        if (opt.value === '' && opt.disabled) row.classList.add('ph');
        if (i === sel.selectedIndex) { row.classList.add('on'); row.setAttribute('aria-selected', 'true'); }
        row.innerHTML = '<span></span><i class="bi bi-check-lg"></i>';
        row.querySelector('span').textContent = opt.textContent;
        list.appendChild(row);
      });
    }

    function paintFace() {
      var opt = sel.options[sel.selectedIndex];
      var label = opt ? opt.textContent.trim() : '';
      face.querySelector('.t').textContent = label;

      face.classList.toggle('ph', !sel.value);
    }

    function place() {
      var r = face.getBoundingClientRect();
      var wanted = Math.min(272, sel.options.length * 41 + 14);
      var below = window.innerHeight - r.bottom - 10;
      var above = r.top - 10;
      var up = below < wanted && above > below;

      list.style.width = r.width + 'px';
      list.style.left = r.left + 'px';
      if (up) {
        list.style.top = '';
        list.style.bottom = (window.innerHeight - r.top + 7) + 'px';
        list.style.maxHeight = Math.max(120, above) + 'px';
      } else {
        list.style.bottom = '';
        list.style.top = (r.bottom + 7) + 'px';
        list.style.maxHeight = Math.max(120, below) + 'px';
      }
      list.classList.toggle('up', up);
    }

    function open() {
      closeAllLists(list);
      fill();
      list.hidden = false;
      list.classList.add('is-fixed');
      face.setAttribute('aria-expanded', 'true');
      box.classList.add('is-open');
      place();
      var on = list.querySelector('.ms-sel-opt.on');
      if (on) on.scrollIntoView({ block: 'nearest' });

      list.msFollow = place;
      window.addEventListener('scroll', place, true);
      window.addEventListener('resize', place);
    }

    function close() {
      list.hidden = true;
      face.setAttribute('aria-expanded', 'false');
      box.classList.remove('is-open');
      if (list.msFollow) {
        window.removeEventListener('scroll', list.msFollow, true);
        window.removeEventListener('resize', list.msFollow);
        list.msFollow = null;
      }
    }

    function pick(i) {
      var opt = sel.options[i];
      if (!opt || opt.disabled) return;
      sel.selectedIndex = i;
      sel.dispatchEvent(new Event('input', { bubbles: true }));
      sel.dispatchEvent(new Event('change', { bubbles: true }));
      paintFace();
      close();
      face.focus();
    }

    face.addEventListener('click', function () { list.hidden ? open() : close(); });

    list.addEventListener('click', function (e) {
      var row = e.target.closest('.ms-sel-opt');
      if (row) pick(parseInt(row.dataset.i, 10));
    });

    face.addEventListener('keydown', function (e) {
      var k = e.key;
      if (k === 'Escape') { close(); return; }
      if (k === 'Enter' || k === ' ') { e.preventDefault(); list.hidden ? open() : close(); return; }
      if (k !== 'ArrowDown' && k !== 'ArrowUp' && k !== 'Home' && k !== 'End') return;
      e.preventDefault();
      var n = sel.options.length, i = sel.selectedIndex;
      var step = k === 'ArrowDown' ? 1 : (k === 'ArrowUp' ? -1 : 0);
      if (k === 'Home') i = -1, step = 1;
      if (k === 'End') i = n, step = -1;
      for (var j = i + step; j >= 0 && j < n; j += step) {
        if (!sel.options[j].disabled) { pick(j); break; }
      }
    });

    sel.addEventListener('change', paintFace);

    sel.addEventListener('invalid', function () {
      box.classList.add('err');
      setTimeout(function () { face.focus({ preventScroll: false }); }, 0);
    });
    sel.addEventListener('change', function () { if (sel.checkValidity()) box.classList.remove('err'); });

    list.msFace = face;
    box.insertBefore(face, sel);
    box.insertBefore(list, sel.nextSibling);
    paintFace();
  }

  document.addEventListener('click', function (e) {
    if (e.target.closest('.ms-sel')) return;
    closeAllLists(null);
  });

  document.addEventListener('keydown', function (e) {
    if (e.key === 'Escape') closeAllLists(null);
  });

  document.querySelectorAll('.m-input select').forEach(buildSelect);

  (function payForm() {
    var number = document.getElementById('card_number');
    if (!number) return;

    var form = number.closest('form');
    var expiry = document.getElementById('card_expiry');
    var cvc = document.getElementById('card_cvc');
    var name = document.getElementById('card_name');
    var submit = form && form.querySelector('button[type=submit], input[type=submit]');

    var preview = form && form.querySelector('.cardbox');
    var pvNumber = preview && preview.querySelector('.cb-num');
    var pvName = preview && preview.querySelector('.cb-bot span:first-child');
    var pvExpiry = preview && preview.querySelector('.cb-bot span:last-child');
    var pvBrand = preview && preview.querySelector('.cb-top i');

    var BRANDS = [
      { re: /^4/, icon: 'bi-credit-card-2-front', label: 'Visa' },
      { re: /^5[1-5]/, icon: 'bi-credit-card-2-front', label: 'Mastercard' },
      { re: /^(50|6)/, icon: 'bi-credit-card', label: 'mada' },
      { re: /^3[47]/, icon: 'bi-credit-card', label: 'American Express' }
    ];

    function digits(v) { return (v || '').replace(/\D/g, ''); }

    function say(input, message) {
      var fg = input.closest('.m-fg') || input.parentElement;
      var box = input.closest('.m-input');
      var note = fg.querySelector('.m-err[data-ms-live]');

      if (message) {
        if (!note) {
          note = document.createElement('p');
          note.className = 'm-err';
          note.setAttribute('data-ms-live', '1');
          note.innerHTML = '<i class="bi bi-exclamation-circle"></i> <span></span>';

          fg.appendChild(note);
        }
        note.querySelector('span').textContent = message;
        note.hidden = false;
        if (box) { box.classList.add('err'); box.classList.remove('ok'); }
      } else {
        if (note) note.hidden = true;
        if (box) {
          box.classList.remove('err');
          box.classList.add(input.value ? 'ok' : '');
          if (!input.value) box.classList.remove('ok');
        }
      }

      var old = fg.querySelector('.m-err:not([data-ms-live])');
      if (old && input.dataset.msTouched === '1') old.hidden = true;
    }

    function checkNumber(live) {
      var d = digits(number.value);
      if (!d) return live ? '' : 'Enter the 16 digits of your card.';
      if (d.length < 16) return live ? '' : 'Enter all 16 digits of your card.';
      return '';
    }

    function checkExpiry(live) {
      var d = digits(expiry && expiry.value);
      if (!d) return live ? '' : 'Enter the expiry date as MM / YY.';
      if (d.length < 4) return live ? '' : 'Enter the expiry date as MM / YY.';
      var mm = parseInt(d.slice(0, 2), 10), yy = parseInt(d.slice(2), 10);
      if (mm < 1 || mm > 12) return 'The month has to be between 01 and 12.';
      var now = new Date();
      var endOfMonth = new Date(2000 + yy, mm, 0, 23, 59, 59);
      if (endOfMonth < now) return 'This card has expired. Use another card.';
      return '';
    }

    function checkCvc(live) {
      var d = digits(cvc && cvc.value);
      if (!d) return live ? '' : 'Enter the security code from the back of the card.';
      if (d.length < 3) return live ? '' : 'The security code is 3 or 4 digits.';
      return '';
    }

    function checkName(live) {
      var v = (name && name.value || '').trim();
      if (!v) return live ? '' : 'Enter the name printed on the card.';
      if (v.length < 3) return live ? '' : 'Enter the full name printed on the card.';
      if (!/^[\p{L}\s\-'.]+$/u.test(v)) return 'Use letters only, as printed on the card.';
      return '';
    }

    var FIELDS = [
      { el: number, check: checkNumber },
      { el: expiry, check: checkExpiry },
      { el: cvc, check: checkCvc },
      { el: name, check: checkName }
    ].filter(function (f) { return f.el; });

    function mirror() {
      if (!preview) return;
      var d = digits(number.value);
      if (pvNumber) {
        var out = '';
        for (var i = 0; i < 16; i++) {
          out += i < d.length ? d[i] : '•';
          if (i % 4 === 3 && i < 15) out += '  ';
        }
        pvNumber.textContent = out;
      }
      if (pvExpiry) {
        var e = digits(expiry && expiry.value);
        pvExpiry.textContent = e.length >= 3 ? e.slice(0, 2) + ' / ' + e.slice(2) : (e.length ? e + ' / YY' : 'MM / YY');
      }
      if (pvName && name) {
        pvName.textContent = (name.value || name.placeholder || '').toUpperCase();
      }
      if (pvBrand) {
        var hit = null;
        BRANDS.forEach(function (b) { if (!hit && b.re.test(d)) hit = b; });
        pvBrand.className = 'bi ' + (hit ? hit.icon : 'bi-credit-card-2-front');
        pvBrand.setAttribute('title', hit ? hit.label : '');
      }
    }

    function paint(live) {
      var firstBad = null;
      FIELDS.forEach(function (f) {
        var msg = f.check(live && f.el.dataset.msTouched !== '1' ? true : live);
        say(f.el, msg);
        if (!firstBad && f.check(false)) firstBad = f.el;
      });
      if (submit) {
        var ready = FIELDS.every(function (f) { return !f.check(false); });
        submit.classList.toggle('is-waiting', !ready);
      }
      return firstBad;
    }

    FIELDS.forEach(function (f) {
      f.el.addEventListener('input', function () { mirror(); paint(true); });
      f.el.addEventListener('blur', function () {
        f.el.dataset.msTouched = '1';
        say(f.el, f.check(false));
        paint(true);
      });
    });

    if (form) {
      form.addEventListener('submit', function (e) {
        FIELDS.forEach(function (f) { f.el.dataset.msTouched = '1'; });
        var bad = paint(false);
        if (bad) {
          e.preventDefault();
          e.stopPropagation();
          bad.focus();
          bad.scrollIntoView({ block: 'center', behavior: 'smooth' });
          if (window.msToast) window.msToast('error', 'Check the card details before paying.');
        }
      }, true);
    }

    mirror();
  })();

  var openDrawer = null;

  function drawerClose() {
    if (!openDrawer) return;
    var d = openDrawer;
    openDrawer = null;
    d.classList.remove('is-open');
    document.body.classList.remove('ms-drawer-open');
    if (d.msOpener) { try { d.msOpener.focus({ preventScroll: true }); } catch (e) {} }

    if (window.location.hash === '#' + d.id) {
      history.replaceState(null, '', window.location.pathname + window.location.search);
    }
  }

  function drawerOpen(d, opener) {
    if (openDrawer && openDrawer !== d) drawerClose();
    openDrawer = d;
    d.msOpener = opener || null;
    d.classList.add('is-open');
    document.body.classList.add('ms-drawer-open');

    var first = d.querySelector('.ms-drawer-b select, .ms-drawer-b input, .ms-drawer-b a, .ms-x');
    if (first) setTimeout(function () { try { first.focus({ preventScroll: true }); } catch (e) {} }, 60);
  }

  document.addEventListener('click', function (e) {
    var opener = e.target.closest('[data-ms-drawer]');
    if (opener) {
      var d = document.getElementById(opener.getAttribute('data-ms-drawer'));
      if (d) { e.preventDefault(); drawerOpen(d, opener); }
      return;
    }
    if (e.target.closest('[data-ms-drawer-close]')) {
      e.preventDefault();
      drawerClose();
      return;
    }

    if (openDrawer && e.target.classList.contains('ms-drawer-veil')) {
      e.preventDefault();
      drawerClose();
    }
  });

  document.addEventListener('keydown', function (e) {
    if (e.key === 'Escape' && openDrawer) {

      if (document.querySelector('.ms-sel-list:not([hidden])')) return;
      drawerClose();
    }
  });

  document.addEventListener('focusin', function (e) {
    if (!openDrawer || openDrawer.contains(e.target)) return;
    if (e.target.closest('.ms-sel-list, .ms-dialog, .ms-toasts')) return;
    var first = openDrawer.querySelector('.ms-x, .ms-drawer-b select, .ms-drawer-b input');
    if (first) { try { first.focus({ preventScroll: true }); } catch (err) {} }
  });

  (function fromHash() {
    if (!window.location.hash) return;
    var d = document.querySelector('.ms-drawer' + window.location.hash);
    if (d) drawerOpen(d, null);
  })();

  (function rail() {
    var KEY = 'masar:rail';
    var buttons = document.querySelectorAll('[data-ms-rail]');
    if (!buttons.length) return;

    function paint(folded) {
      document.body.classList.toggle('rail-min', folded);
      buttons.forEach(function (b) {
        b.setAttribute('aria-expanded', folded ? 'false' : 'true');
        b.setAttribute('aria-label', folded ? 'Open the menu' : 'Fold the menu');
      });
    }

    buttons.forEach(function (b) {
      b.addEventListener('click', function () {
        var folded = !document.body.classList.contains('rail-min');
        paint(folded);
        try { localStorage.setItem(KEY, folded ? '1' : '0'); } catch (e) {}
      });
    });

    paint(document.body.classList.contains('rail-min'));
  })();

  wireAllReveals(document);
})();
