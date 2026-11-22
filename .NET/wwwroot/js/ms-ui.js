(function () {
  var body = document.body;
  function setNav(open) {
    body.classList.toggle('md-nav-open', open);
    document.querySelectorAll('[data-md-toggle]').forEach(function (b) { b.setAttribute('aria-expanded', open ? 'true' : 'false'); });
  }
  function closePops(except) {
    document.querySelectorAll('.ms-menu:not([hidden])').forEach(function (m) { if (m !== except) m.hidden = true; });
  }

  document.addEventListener('click', function (e) {
    var t = e.target;
    if (t.closest('[data-md-toggle]')) { setNav(!body.classList.contains('md-nav-open')); return; }
    if (t.closest('.md-backdrop') || t.closest('.dnav a, .m-nav a, .app-nav a')) setNav(false);
    else if (body.classList.contains('md-nav-open') && !t.closest('.dsb, .m-nav, .app-nav')) setNav(false);

    var pop = t.closest('[data-ms-pop]');
    if (pop && !t.closest('.ms-menu')) {
      var m = document.getElementById(pop.getAttribute('data-ms-pop'));
      if (m) { closePops(m); m.hidden = !m.hidden; e.preventDefault(); return; }
    }
    if (!t.closest('.ms-menu')) closePops(null);

    var opener = t.closest('[data-ms-open]');
    if (opener) {
      var dlg = document.getElementById(opener.getAttribute('data-ms-open'));
      if (dlg) { dlg.hidden = false; e.preventDefault(); var f = dlg.querySelector('input, textarea, select, button'); if (f) f.focus(); }
      return;
    }
    if (t.closest('[data-ms-close]') || t.classList.contains('ms-dialog')) {
      var d = t.closest('.ms-dialog'); if (d) { d.hidden = true; e.preventDefault(); }
    }
  });
  document.addEventListener('keydown', function (e) {
    if (e.key !== 'Escape') return;
    setNav(false); closePops(null);
    document.querySelectorAll('.ms-dialog:not([hidden])').forEach(function (d) { d.hidden = true; });
  });
  window.addEventListener('resize', function () { if (window.innerWidth >= 992) setNav(false); });

  document.querySelectorAll('[data-ms-password]').forEach(function (input) {
    var hint = document.getElementById(input.getAttribute('data-ms-password'));
    var scope = input.closest('form') || document;
    function paint() {
      var v = input.value, ok = { len: v.length >= 8, num: /\d/.test(v), cap: /[A-Z]/.test(v), low: /[a-z]/.test(v) };
      scope.querySelectorAll('[data-ms-rule]').forEach(function (r) {
        var good = ok[r.getAttribute('data-ms-rule')];
        r.className = good ? 'bi bi-check-circle-fill' : 'bi bi-circle';
        r.style.color = good ? 'var(--sage)' : 'var(--mint-300)';
      });
      if (!hint) return;
      if (!v) { hint.textContent = hint.getAttribute('data-default') || ''; hint.className = 'm-help'; return; }
      var strong = ok.len && ok.num && ok.cap && ok.low;
      hint.className = strong ? 'm-help' : 'm-err';
      hint.textContent = strong
        ? (v.length >= 12 ? 'Strong, ' : 'Good, ') + v.length + ' characters, letters and numbers.'
        : 'Use at least 8 characters with a capital letter, a small letter and a number.';
    }
    if (hint && !hint.hasAttribute('data-default')) hint.setAttribute('data-default', hint.textContent);
    input.addEventListener('input', paint);
  });

  document.querySelectorAll('[data-ms-count]').forEach(function (el) {
    var out = document.getElementById(el.getAttribute('data-ms-count')), max = parseInt(el.getAttribute('maxlength'), 10) || 1000;
    function paint() { if (out) out.textContent = el.value.length.toLocaleString('en-US') + ' of ' + max.toLocaleString('en-US') + ' characters'; }
    el.addEventListener('input', paint); paint();
  });

  document.querySelectorAll('[data-ms-phone]').forEach(function (i) {
    i.addEventListener('input', function () {
      var d = i.value.replace(/\D/g, '');
      if (d.indexOf('05') === 0) d = '966' + d.slice(1);
      if (d.indexOf('00966') === 0) d = d.slice(2);
      if (d.indexOf('966') !== 0) { i.value = i.value.replace(/[^\d+ ]/g, ''); return; }
      d = d.slice(3, 12);
      i.value = '+966' + (d ? ' ' + d.slice(0, 2) : '') + (d.length > 2 ? ' ' + d.slice(2, 5) : '') + (d.length > 5 ? ' ' + d.slice(5) : '');
    });
  });

  document.querySelectorAll('[data-ms-card]').forEach(function (i) {
    i.addEventListener('input', function () { i.value = i.value.replace(/\D/g, '').slice(0, 16).replace(/(\d{4})(?=\d)/g, '$1 '); });
  });
  document.querySelectorAll('[data-ms-expiry]').forEach(function (i) {
    i.addEventListener('input', function () { var d = i.value.replace(/\D/g, '').slice(0, 4); i.value = d.length > 2 ? d.slice(0, 2) + ' / ' + d.slice(2) : d; });
  });

  document.querySelectorAll('[data-ms-countdown]').forEach(function (el) {
    var left = parseInt(el.getAttribute('data-ms-countdown'), 10) || 0;
    function show() { el.textContent = Math.ceil(left / 60); }
    show();
    var timer = setInterval(function () { left--; show(); if (left <= 0) { clearInterval(timer); window.location.reload(); } }, 1000);
  });

  document.querySelectorAll('label.ms-drop').forEach(function (zone) {
    var input = zone.querySelector('input[type=file]'), list = document.querySelector('[data-ms-files="' + (input && input.name) + '"]');
    function render() {
      if (!list || !input) return;
      list.innerHTML = '';
      Array.prototype.forEach.call(input.files, function (f) {
        var s = document.createElement('span'); s.className = 'file';
        s.innerHTML = '<i class="bi bi-paperclip"></i> ' + f.name.replace(/[<>&]/g, '') + ' <span class="m-small">' + (f.size / 1048576).toFixed(1) + ' MB</span>';
        list.appendChild(s);
      });
    }
    ['dragenter', 'dragover'].forEach(function (ev) { zone.addEventListener(ev, function (e) { e.preventDefault(); zone.classList.add('is-drag'); }); });
    ['dragleave', 'drop'].forEach(function (ev) { zone.addEventListener(ev, function (e) { e.preventDefault(); zone.classList.remove('is-drag'); }); });
    zone.addEventListener('drop', function (e) { if (input && e.dataTransfer.files.length) { input.files = e.dataTransfer.files; render(); } });
    if (input) input.addEventListener('change', render);
  });
})();
