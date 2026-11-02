using System;
using System.Collections.Generic;
using System.Text;
using VisumCRM.Data;

namespace VisumCRM.Repository
{
    public class LocationRepository
    {
        public delegate void LocationEventHandler(object sender, LocationEventArgs e);
        public event LocationEventHandler? LocationCreated;
        public event LocationEventHandler? LocationUpdated;
        public event LocationEventHandler? LocationDeleted;
        private Data.ApiContext _context;
        private Guid _license;
        public LocationRepository(ApiContext Context, Guid license)
        {
            _context = Context;
            _license = license;
        }

        public Entity.Location? Get(long Id)
        {
            if (_context == null) return null;
            return _context.Locations.FirstOrDefault(x => x.Id == Id && !x.IsDeleted && x.LicenseKey == _license);
        }
        public Entity.Location? GetByPostalzoneStreetHouse(long PostalzoneId, string Street, string House, string? Floor, string? Door)
        {
            if (_context == null) return null;
            return _context.Locations.FirstOrDefault(x => x.PostalzoneId == PostalzoneId && x.Street == Street && x.House == House && x.Floor == Floor && x.Door == Door && !x.IsDeleted && x.LicenseKey == _license);
        }
        public List<Entity.Location>? GetAll()
        {
            if (_context == null) return null;
            return _context.Locations.Where(x => !x.IsDeleted && x.LicenseKey == _license).ToList();
        }
        public Entity.Location? Create(Entity.Location? Source)
        {
            if (_context == null || Source == null) return null;
            Source.LicenseKey = _license;
            _context.Locations.Add(Source);
            _context.SaveChanges();
            LocationCreated?.Invoke(this, new LocationEventArgs(Source));
            return Source;
        }
        public Entity.Location? Update(Entity.Location? Source)
        {
            if (_context == null || Source == null) return null;
            var record = Get(Source.Id);
            if (record == null) return null;
            record.Door = Source.Door;
            record.Floor = Source.Floor;
            record.House = Source.House;
            record.Latitude = Source.Latitude;
            record.Longitude = Source.Longitude;
            record.PostalzoneId = Source.PostalzoneId;
            record.Street = Source.Street;
            _context.SaveChanges();
            LocationUpdated?.Invoke(this, new LocationEventArgs(record));
            return record;
        }
        public Entity.Location? Delete(long Id)
        {
            var record = Get(Id);
            if (_context == null || record == null) return null;
            record.IsDeleted = true;
            _context.SaveChanges();
            LocationDeleted?.Invoke(this, new LocationEventArgs(record));
            return record;
        }
    }

    public class LocationEventArgs
    {
        public Entity.Location? Location { get; set; }
        public LocationEventArgs(Entity.Location? location)
        {
            Location = location;
        }
    }
}
